using FinalEcormer2023.Interfaces;
using FinalEcormer2023.Models;
using FinalEcormer2023.Service;
using FinalEcormer2023.ViewModels;
using FinalEcormer2023.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net;
using System.Reflection.Metadata;

namespace FinalEcormer2023.Controllers {
	public class AccountController : Controller {
		private readonly UserManager<IdentityUser> _userManager;
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly IEmailSender _emailSender;

		public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IEmailSender emailSender) {
			_userManager = userManager;
			_signInManager = signInManager;
			_emailSender = emailSender;
		}

        [HttpGet]
		public async Task<IActionResult> Register(string? returnUrl) {
			RegisterViewModel model = new RegisterViewModel();
			model.ReturnUrl = returnUrl;
			ViewBag.Title = "Register";
			return View(model);
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				var checkEmail = await _userManager.FindByEmailAsync(model.Email);
				var userCheck = await _userManager.FindByNameAsync(model.UserName);

				if (checkEmail != null || userCheck != null)
				{
					ViewBag.ErrorMessage = "Account Exist!";
					return View();
				}

				var user = new AppUser { Email = model.Email, UserName = model.UserName };
				var result = await _userManager.CreateAsync(user, model.Password);

				if (result.Succeeded)
				{
					await _signInManager.SignInAsync(user, isPersistent: false);

					string role_Users = "User";
					await _userManager.AddToRoleAsync(user, role_Users);

					string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
					var callBackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
					await _emailSender.SendEmailAsync(model.Email, "xác thực tài khoản ", $"vui lòng click <a href=\"{callBackUrl}\">đây</a>");

					ViewBag.Success = "Send Email please check your email";
					return View();
				}

				ModelState.AddModelError("Password", "User could not be created");
			}

			return View();
		}

		[HttpGet]
		public async Task<IActionResult> ConfirmEmail(String userId, string code) {
			if (userId == null || code == null) {
				return RedirectToAction("Index", "Home");
			}
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null) {
			}
			var result = await _userManager.ConfirmEmailAsync(user, code);
			if (result.Succeeded) {
				ViewBag.Title = "Confirm Register Successful";
			} else {
				ViewBag.Title = "Confirm Register falure";
			}
			return View();
		}

        [HttpGet]
		public async Task<IActionResult> Login(string? returnUrl = null) {
			LoginViewModel model = new LoginViewModel();
			model.ReturnUrl = returnUrl ?? Url.Content("~/");
			return View(model);
		}
		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, false);

			if (result.Succeeded)
			{
				var user = await _userManager.FindByNameAsync(model.Username);

				if (user != null)
				{
					var roles = await _userManager.GetRolesAsync(user);

					if (roles.Contains("Admin"))
					{
						return RedirectToAction("Index", "Admin");
					}
					else if (roles.Contains("User"))
					{
						return RedirectToAction("Index", "Home");
					}
				}

				// Nếu không có vai trò cụ thể, mặc định chuyển hướng đến trang Home
				return RedirectToAction("Index", "Home");
			}

			ModelState.AddModelError("Password", "Invalid Login Attempt");
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logoff() {
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public IActionResult ChangePassword() {
			ChangePasswordViewModel changePasswordViewModel = new ChangePasswordViewModel();
			return View(changePasswordViewModel);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model) {
			if (!ModelState.IsValid) {
				return View(model);
			}
			var user = await _userManager.GetUserAsync(User);
			var result = await _userManager.ChangePasswordAsync(user, model.oldPassword, model.newPassword);
			if (result.Succeeded) {
				if (user != null) {
					await _signInManager.SignInAsync(user, isPersistent: false);
				}
				ViewBag.Success = "Change password successful";
				return View(model);
			}
			ModelState.AddModelError(String.Empty, "Password currently not right!");
			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> ForgotPassword() {
			ForgotPasswordViewModel model = new ForgotPasswordViewModel();
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model) {
			if (ModelState.IsValid) {
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user == null || !(await _userManager.IsEmailConfirmedAsync(user))) {
					ViewBag.Message = "Account not exist or Account not confirm!";
					return View("ForgotPasswordConfirm");
				}
				string code = await _userManager.GeneratePasswordResetTokenAsync(user);
				var callBackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, statusCode = code }, protocol: HttpContext.Request.Scheme);
				await _emailSender.SendEmailAsync(model.Email, "Reset password ", "vui lòng click <a href=\"" + callBackUrl + "\">đây </a>");
				ViewBag.Message = "Check your email to get reset password!";
				return View("ForgotPasswordConfirm");
			}
			return View(model);
		}

		[HttpGet]
		public IActionResult ResetPassword(string statusCode = null) {
			ViewBag.Title = "Reset password";
			if (statusCode == null) {
				return View();
			} else {
				ResetPasswordViewModel model = new ResetPasswordViewModel();
				model.StatusCode = statusCode;
				return View(model);
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel) {
			if (ModelState.IsValid) {
				var user = await _userManager.FindByEmailAsync(resetPasswordViewModel.Email);
				if (user == null) {
					ModelState.AddModelError("Email", "User not found");
					return View(resetPasswordViewModel);
				}
				var result = await _userManager.ResetPasswordAsync(user, resetPasswordViewModel.StatusCode, resetPasswordViewModel.newPassword);
				if (result.Succeeded) {
					return RedirectToAction("ResetPasswordConfirm");
				}
			}
			return View(resetPasswordViewModel);
		}

		[HttpGet]
		public IActionResult ResetPasswordConfirm() {
			return View();
		}
	}
}
