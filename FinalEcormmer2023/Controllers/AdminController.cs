using Microsoft.AspNetCore.Mvc;
using FinalEcormer2023.Interfaces;
using FinalEcormmer2023.Controllers;
using Microsoft.AspNetCore.Authorization;
using FinalEcormer2023.Models;
using FinalEcormer2023.Views.Shared.Components.SearchBar;
using FinalEcormer2023.Views.Shared.Components.PageDivide;
using FinalEcormer2023.Data;
using FinalEcormer2023.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using FinalEcormer2023.ViewModels;
using Microsoft.EntityFrameworkCore;
using Unidecode;
using FinalEcormer2023.Extensions;
using static NuGet.Packaging.PackagingConstants;
using Unidecode.NET;
using System.Security.Claims;

namespace FinalEcormer2023.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
		private readonly ILogger<AdminController> _logger;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IProductService _productService;
		private readonly ICategoryService _categoryService;
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly Interfaces.IEmailSender _emailSender;
		private readonly ApplicationDbContext _context;

		public AdminController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, Interfaces.IEmailSender emailSender, ILogger<AdminController> logger, IProductService productService, ICategoryService categoryService, IOrderService orderService, IOrderDetailService orderDetailService, ApplicationDbContext context)
		{
			_logger = logger;
			_userManager = userManager;
			_signInManager = signInManager;
            _roleManager = roleManager;
			_emailSender = emailSender;
			_productService = productService;
			_categoryService = categoryService;
            _orderService = orderService;
            _orderDetailService = orderDetailService;
			_context = context;
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Index(string SearchText = "", int pg = 1)
        {
            IEnumerable<Product> products;

            if (string.IsNullOrEmpty(SearchText))
            {
                products = await _productService.GetAll();
            }
            else
            {
                var allProducts = await _productService.GetAll();
                products = allProducts.Where(p => p.Name.Unidecode().IndexOf(SearchText.Unidecode(), StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }

            const int pageSize = 5;
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = products.Count();
            int recsSkip = (pg - 1) * pageSize;
            var data = products.Skip(recsSkip).Take(pageSize).ToList();

            var categories = await _categoryService.GetAll();
            ViewBag.Categories = categories;

            products = products.Select(p =>
            {
                p.category = categories.FirstOrDefault(c => c.Id == p.CategoryId);
                return p;
            }).ToList();


            Views.Shared.Components.SearchBar.SPager SearchPager = new Views.Shared.Components.SearchBar.SPager(recsCount, pg, pageSize) { Action = "Index", Controller = "Admin", SearchText = SearchText };
            ViewBag.SearchPager = SearchPager;
            Views.Shared.Components.PageDivide.SPager PageDivide = new Views.Shared.Components.PageDivide.SPager(recsCount, pg, pageSize) { Action = "Index", Controller = "Admin", SearchText = SearchText };
            ViewBag.PageDivide = PageDivide;

            return View(data);
        }

		public async Task<IActionResult> Category(string SearchText = "", int pg = 1)
		{
			IEnumerable<Category> categories;

			if (string.IsNullOrEmpty(SearchText))
			{
				categories = await _categoryService.GetAll();
			}
			else
			{
				var allCategories = await _categoryService.GetAll();
				categories = allCategories.Where(c => c.Name.Unidecode().IndexOf(SearchText.Unidecode(), StringComparison.OrdinalIgnoreCase) >= 0).ToList();
			}

			const int pageSize = 5;
			if (pg < 1)
			{
				pg = 1;
			}
			int recsCount = categories.Count();
			int recsSkip = (pg - 1) * pageSize;
			var data = categories.Skip(recsSkip).Take(pageSize).ToList();
			Views.Shared.Components.SearchBar.SPager SearchPager = new Views.Shared.Components.SearchBar.SPager(recsCount, pg, pageSize) { Action = "Category", Controller = "Admin", SearchText = SearchText };
			ViewBag.SearchPager = SearchPager;
			Views.Shared.Components.PageDivide.SPager PageDivide = new Views.Shared.Components.PageDivide.SPager(recsCount, pg, pageSize) { Action = "Category", Controller = "Admin", SearchText = SearchText };
			ViewBag.PageDivide = PageDivide;

			return View(data);
		}

        [HttpPost]
        public async Task<IActionResult> AddCategory(string categoryName)
        {
            try
            {
                var existingCategory = (await _categoryService.GetAll())
                    .FirstOrDefault(c => string.Equals(c.Name.Trim(), categoryName.Trim(), StringComparison.OrdinalIgnoreCase));

                if (existingCategory != null)
                {
                    return BadRequest("Category already exists");
                }
                else
                {
                    var newCategory = new Category { Name = categoryName };
                    await _categoryService.Add(newCategory);
                    return Ok("Category added successfully");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategory(int categoryId, string updatedCategoryName)
        {
            try
            {
                var categoryToUpdate = await _categoryService.GetById(categoryId);

                if (categoryToUpdate != null)
                {
                    var existingCategory = (await _categoryService.GetAll())
                        .FirstOrDefault(c => string.Equals(c.Name.Trim(), updatedCategoryName.Trim(), StringComparison.OrdinalIgnoreCase));

                    if (existingCategory != null)
                    {
                        return BadRequest("Category already exists");
                    }
                    else
                    {
                        categoryToUpdate.Name = updatedCategoryName;
                        await _categoryService.Update(categoryToUpdate);
                        return Ok("Category updated successfully");
                    }
                }
                else
                {
                    return BadRequest("Category not found");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var categoryToDelete = await _categoryService.GetById(id);

                if (categoryToDelete != null)
                {
                    var productsInCategory = await _productService.GetProductsByCategoryId(id);

                    if (productsInCategory.Any())
                    {
                        return BadRequest("There are associated products. Cannot delete the category.");
                    }

                    await _categoryService.Delete(categoryToDelete);
                    return Ok("Category deleted successfully");
                }
                else
                {
                    return BadRequest("Category not found.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("An unexpected error occurred.");
            }
        }

        public async Task<IActionResult> Product(string SearchText = "", int pg = 1)
        {
            IEnumerable<Product> products;

            if (string.IsNullOrEmpty(SearchText))
            {
                products = await _productService.GetAll();
            }
            else
            {
                var allProducts = await _productService.GetAll();
                products = allProducts.Where(p => p.Name.Unidecode().IndexOf(SearchText.Unidecode(), StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }

            const int pageSize = 5;
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = products.Count();
            int recsSkip = (pg - 1) * pageSize;
            var data = products.Skip(recsSkip).Take(pageSize).ToList();

            var categories = await _categoryService.GetAll();
            ViewBag.Categories = categories;

            products = products.Select(p =>
            {
                p.category = categories.FirstOrDefault(c => c.Id == p.CategoryId);
                return p;
            }).ToList();


            Views.Shared.Components.SearchBar.SPager SearchPager = new Views.Shared.Components.SearchBar.SPager(recsCount, pg, pageSize) { Action = "Product", Controller = "Admin", SearchText = SearchText };
            ViewBag.SearchPager = SearchPager;
            Views.Shared.Components.PageDivide.SPager PageDivide = new Views.Shared.Components.PageDivide.SPager(recsCount, pg, pageSize) { Action = "Product", Controller = "Admin", SearchText = SearchText };
            ViewBag.PageDivide = PageDivide;

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(string productName, string productColor, string productDescription, string productUrl, decimal productPrice, int categoryId)
        {
            try
            {
                var existingCategory = (await _categoryService.GetAll())
                    .FirstOrDefault(c => c.Id == categoryId);

                if (existingCategory == null)
                {
                    return BadRequest("Category doesn't exist.");
                }

                var existingProduct = (await _productService.GetAll())
                    .FirstOrDefault(p => string.Equals(p.Name, productName, StringComparison.OrdinalIgnoreCase));

                if (existingProduct != null)
                {
                    return BadRequest("Product already exists.");
                }

                Product newProduct = new Product
                {
                    Name = productName,
                    Color = productColor,
                    Description = productDescription,
                    Url = productUrl,
                    Price = productPrice,
                    CategoryId = categoryId
                };

                await _productService.Add(newProduct);
                return Ok("Product added successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest("An unexpected error occurred.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(int id, string newName, string newColor, string newDescription, string newUrl, decimal newPrice, int newCategoryId)
        {
            try
            {
                var existingCategory = (await _categoryService.GetAll())
                                    .FirstOrDefault(c => c.Id == newCategoryId);

                if (existingCategory == null)
                {
                    return BadRequest("Category doesn't exist.");
                }

                var existingProduct = (await _productService.GetAll())
                    .FirstOrDefault(p => p.Id != id && string.Equals(p.Name, newName, StringComparison.OrdinalIgnoreCase));

                if (existingProduct != null)
                {
                    return BadRequest("Product name already exists.");
                }

                var productToUpdate = await _productService.GetById(id);

                if (productToUpdate != null)
                {
                    productToUpdate.Name = newName;
                    productToUpdate.Color = newColor;
                    productToUpdate.Description = newDescription;
                    productToUpdate.Url = newUrl;
                    productToUpdate.Price = newPrice;
                    productToUpdate.CategoryId = newCategoryId;

                    await _productService.Update(productToUpdate);
                    return Ok("Product updated successfully");
                }
                else
                {
                    return BadRequest("Product not found.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("An unexpected error occurred.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var productToDelete = await _productService.GetById(id);

                if (productToDelete != null)
                {
                    try
                    {
                        var orderDetailsInProduct = await _orderDetailService.GetOrderDetailsByProductId(id);

                        if (orderDetailsInProduct.Any())
                        {
                            return BadRequest("There are associated order details. Cannot delete the product.");
                        }

                        await _productService.Delete(productToDelete);
                        return Ok("Product deleted successfully");
                    }
                    catch (Exception ex)
                    {
                        return BadRequest("Error deleting product");
                    }
                }
                else
                {
                    return BadRequest("Product not found.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("An unexpected error occurred.");
            }
        }

        public async Task<IActionResult> ProductByCategory(int id, string SearchText = "", int pg = 1)
        {
            IEnumerable<Product> products;

            if (string.IsNullOrEmpty(SearchText))
            {
                products = await _productService.GetProductsByCategoryId(id);
            }
            else
            {
                var allProducts = await _productService.GetProductsByCategoryId(id);
                products = allProducts.Where(p => p.Name.Unidecode().IndexOf(SearchText.Unidecode(), StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }

            const int pageSize = 5;
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = products.Count();
            int recsSkip = (pg - 1) * pageSize;
            var data = products.Skip(recsSkip).Take(pageSize).ToList();

            Views.Shared.Components.SearchBar.SPager SearchPager = new Views.Shared.Components.SearchBar.SPager(recsCount, pg, pageSize) { Action = "ProductByCategory", Controller = "Admin", SearchText = SearchText };
            ViewBag.SearchPager = SearchPager;
            Views.Shared.Components.PageDivide.SPager PageDivide = new Views.Shared.Components.PageDivide.SPager(recsCount, pg, pageSize) { Action = "ProductByCategory", Controller = "Admin", SearchText = SearchText };
            ViewBag.PageDivide = PageDivide;

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProductByCategory(int id, string newName, string newColor, string newDescription, string newUrl, decimal newPrice)
        {
            try
            {
                var existingProduct = (await _productService.GetAll())
                    .FirstOrDefault(p => p.Id != id && string.Equals(p.Name, newName, StringComparison.OrdinalIgnoreCase));

                if (existingProduct != null)
                {
                    return BadRequest("Product name already exists.");
                }

                var productToUpdate = await _productService.GetById(id);

                if (productToUpdate != null)
                {
                    productToUpdate.Name = newName;
                    productToUpdate.Color = newColor;
                    productToUpdate.Description = newDescription;
                    productToUpdate.Url = newUrl;
                    productToUpdate.Price = newPrice;

                    await _productService.Update(productToUpdate);
                    return Ok("Product updated successfully");
                }
                else
                {
                    return BadRequest("Product not found.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("An unexpected error occurred.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProductByCategory(int id)
        {
            try
            {
                var productToDelete = await _productService.GetById(id);

                if (productToDelete != null)
                {
                    try
                    {
                        var orderDetailsInProduct = await _orderDetailService.GetOrderDetailsByProductId(id);

                        if (orderDetailsInProduct.Any())
                        {
                            return BadRequest("There are associated order details. Cannot delete the product.");
                        }

                        await _productService.Delete(productToDelete);
                        return Ok("Product deleted successfully");
                    }
                    catch (Exception ex)
                    {
                        return BadRequest("Error deleting product");
                    }
                }
                else
                {
                    return BadRequest("Product not found.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("An unexpected error occurred.");
            }
        }

        public async Task<IActionResult> User(string searchText = "", int pg = 1)
        {
            var users = await _userManager.Users
                .Where(u => string.IsNullOrEmpty(searchText) || u.UserName.Contains(searchText))
                .ToListAsync();

            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.FirstOrDefault() ?? "User";

                userViewModels.Add(new UserViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = role
                });
            }

            const int pageSize = 5;
            if (pg < 1)
            {
                pg = 1;
            }
            var recsCount = userViewModels.Count;
            var recsSkip = (pg - 1) * pageSize;
            var data = userViewModels.Skip(recsSkip).Take(pageSize).ToList();

            var menuroles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

            ViewBag.Roles = menuroles;

            Views.Shared.Components.SearchBar.SPager SearchPager = new Views.Shared.Components.SearchBar.SPager(recsCount, pg, pageSize) { Action = "User", Controller = "Admin", SearchText = searchText };
            ViewBag.SearchPager = SearchPager;
            Views.Shared.Components.PageDivide.SPager PageDivide = new Views.Shared.Components.PageDivide.SPager(recsCount, pg, pageSize) { Action = "User", Controller = "Admin", SearchText = searchText };
            ViewBag.PageDivide = PageDivide;

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(string id, string newUserName, string email, string role)
        {
            try
            {
                var existingUser = await _userManager.FindByIdAsync(id);

                if (existingUser != null)
                {
                    var currentUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    var currentUser = await _userManager.FindByIdAsync(currentUserId);

                    if (currentUser != null && currentUser.UserName.Equals("admin", StringComparison.OrdinalIgnoreCase))
                    {
                        if (existingUser.UserName.Equals("admin", StringComparison.OrdinalIgnoreCase) && existingUser.Id == currentUserId)
                        {
                            return BadRequest("Cannot update your own account.");
                        }

                        existingUser.UserName = newUserName;
                        existingUser.Email = email;

                        var result = await _userManager.UpdateAsync(existingUser);

                        if (result.Succeeded)
                        {
                            if (!string.IsNullOrEmpty(role) && !await _userManager.IsInRoleAsync(existingUser, role))
                            {
                                var currentRoles = await _userManager.GetRolesAsync(existingUser);
                                await _userManager.RemoveFromRolesAsync(existingUser, currentRoles);

                                await _userManager.AddToRoleAsync(existingUser, role);
                            }

                            return Ok("User updated successfully");
                        }
                        else
                        {
                            return BadRequest("Error updating user");
                        }
                    }
                    else
                    {
                        var userRoles = await _userManager.GetRolesAsync(existingUser);

                        if (userRoles.Contains("Admin"))
                        {
                            return BadRequest("You don't have permission to update admin users.");
                        }

                        existingUser.UserName = newUserName;
                        existingUser.Email = email;

                        var result = await _userManager.UpdateAsync(existingUser);

                        if (result.Succeeded)
                        {
                            if (!string.IsNullOrEmpty(role) && !await _userManager.IsInRoleAsync(existingUser, role))
                            {
                                var currentRoles = await _userManager.GetRolesAsync(existingUser);
                                await _userManager.RemoveFromRolesAsync(existingUser, currentRoles);

                                await _userManager.AddToRoleAsync(existingUser, role);
                            }

                            return Ok("User updated successfully");
                        }
                        else
                        {
                            return BadRequest("Error updating user");
                        }
                    }
                }
                else
                {
                    return BadRequest("User not found");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("An unexpected error occurred.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                var userToDelete = await _userManager.FindByIdAsync(id);

                if (userToDelete != null)
                {
                    var currentUserId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                    if (id == currentUserId)
                    {
                        return BadRequest("Cannot delete your own account.");
                    }

                    var currentUser = await _userManager.FindByIdAsync(currentUserId);

                    if (currentUser != null && currentUser.UserName.Equals("admin", StringComparison.OrdinalIgnoreCase))
                    {
                        var result = await _userManager.DeleteAsync(userToDelete);

                        if (result.Succeeded)
                        {
                            return Ok("User deleted successfully");
                        }
                        else
                        {
                            return BadRequest("Error deleting user");
                        }
                    }
                    else
                    {
                        var userRoles = await _userManager.GetRolesAsync(userToDelete);

                        if (userRoles.Contains("Admin"))
                        {
                            return BadRequest("Cannot delete admin.");
                        }

                        var result = await _userManager.DeleteAsync(userToDelete);

                        if (result.Succeeded)
                        {
                            return Ok("User deleted successfully");
                        }
                        else
                        {
                            return BadRequest("Error deleting user");
                        }
                    }
                }
                else
                {
                    return BadRequest("User not found.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("An unexpected error occurred.");
            }
        }

        public async Task<IActionResult> Order(string SearchText = "", int pg = 1)
        {
            IEnumerable<Order> orders;

            if (string.IsNullOrEmpty(SearchText))
            {
                orders = await _orderService.GetAll();
            }
            else
            {
                var allOrders = await _orderService.GetAll();
                orders = allOrders.Where(c => c.ShipName.Unidecode().IndexOf(SearchText.Unidecode(), StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }

            const int pageSize = 5;
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = orders.Count();
            int recsSkip = (pg - 1) * pageSize;
            var data = orders.Skip(recsSkip).Take(pageSize).ToList();
            Views.Shared.Components.SearchBar.SPager SearchPager = new Views.Shared.Components.SearchBar.SPager(recsCount, pg, pageSize) { Action = "Order", Controller = "Admin", SearchText = SearchText };
            ViewBag.SearchPager = SearchPager;
            Views.Shared.Components.PageDivide.SPager PageDivide = new Views.Shared.Components.PageDivide.SPager(recsCount, pg, pageSize) { Action = "Order", Controller = "Admin", SearchText = SearchText };
            ViewBag.PageDivide = PageDivide;

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrder(int orderId, string updatedOrderShipName, string updatedOrderShipAddress, string updatedOrderShipEmail, string updatedOrderShipPhoneNumber, decimal updatedOrderTotal)
        {
            try
            {
                var orderToUpdate = await _orderService.GetById(orderId);

                if (orderToUpdate != null)
                {
                    orderToUpdate.ShipName = updatedOrderShipName;
                    orderToUpdate.ShipAddress = updatedOrderShipAddress;
                    orderToUpdate.ShipEmail = updatedOrderShipEmail;
                    orderToUpdate.ShipPhoneNumber = updatedOrderShipPhoneNumber;
                    orderToUpdate.Total = updatedOrderTotal;
                    await _orderService.Update(orderToUpdate);
                    return Ok("Order updated successfully");
                }
                else
                {
                    return BadRequest("Order not found");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                var orderToDelete = await _orderService.GetById(id);

                if (orderToDelete != null)
                {
                    await _orderService.Delete(orderToDelete);
                    return Ok("Order deleted successfully");
                }
                else
                {
                    return BadRequest("Order not found.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("An unexpected error occurred.");
            }
        }

        public async Task<IActionResult> OrderDetailByOrder(int id, string SearchText = "", int pg = 1)
        {
            IEnumerable<OrderDetail> orderDetails;

            if (string.IsNullOrEmpty(SearchText))
            {
                orderDetails = await _orderDetailService.GetOrderDetailsByOrderId(id);
            }
            else
            {
                var allOrderDetails = await _orderDetailService.GetOrderDetailsByOrderId(id);
                orderDetails = allOrderDetails.Where(od => od.Product.Name.Unidecode().IndexOf(SearchText.Unidecode(), StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }

            const int pageSize = 5;
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = orderDetails.Count();
            int recsSkip = (pg - 1) * pageSize;
            var data = orderDetails.Skip(recsSkip).Take(pageSize).ToList();

            Views.Shared.Components.SearchBar.SPager SearchPager = new Views.Shared.Components.SearchBar.SPager(recsCount, pg, pageSize) { Action = "OrderDetailByOrder", Controller = "Admin", SearchText = SearchText };
            ViewBag.SearchPager = SearchPager;
            Views.Shared.Components.PageDivide.SPager PageDivide = new Views.Shared.Components.PageDivide.SPager(recsCount, pg, pageSize) { Action = "OrderDetailByOrder", Controller = "Admin", SearchText = SearchText };
            ViewBag.PageDivide = PageDivide;

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOrderDetailByOrder(int id)
        {
            try
            {
                var orderDetailToDelete = await _orderDetailService.GetById(id);

                if (orderDetailToDelete != null)
                {
                    var order = await _orderService.GetById(orderDetailToDelete.OrderId);

                    await _orderDetailService.Delete(orderDetailToDelete);

                    var remainingOrderDetails = await _orderDetailService.GetOrderDetailsByOrderId(order.Id);

                    if (remainingOrderDetails == null || !remainingOrderDetails.Any())
                    {
                        await _orderService.Delete(order);
                    }

                    return Ok("Order detail deleted successfully");
                }
                else
                {
                    return BadRequest("Order detail not found.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("An unexpected error occurred.");
            }
        }

        public async Task<IActionResult> OrderDetailByProduct(int id, string SearchText = "", int pg = 1)
        {
            IEnumerable<OrderDetail> orderDetails;

            if (string.IsNullOrEmpty(SearchText))
            {
                orderDetails = await _orderDetailService.GetOrderDetailsByProductId(id);
            }
            else
            {
                var allOrderDetails = await _orderDetailService.GetOrderDetailsByProductId(id);
                orderDetails = allOrderDetails
                    .Where(od => od.Order.ShipName.Unidecode().IndexOf(SearchText.Unidecode(), StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }

            const int pageSize = 5;
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = orderDetails.Count();
            int recsSkip = (pg - 1) * pageSize;
            var data = orderDetails.Skip(recsSkip).Take(pageSize).ToList();

            Views.Shared.Components.SearchBar.SPager SearchPager = new Views.Shared.Components.SearchBar.SPager(recsCount, pg, pageSize) { Action = "OrderDetailByProduct", Controller = "Admin", SearchText = SearchText };
            ViewBag.SearchPager = SearchPager;
            Views.Shared.Components.PageDivide.SPager PageDivide = new Views.Shared.Components.PageDivide.SPager(recsCount, pg, pageSize) { Action = "OrderDetailByProduct", Controller = "Admin", SearchText = SearchText };
            ViewBag.PageDivide = PageDivide;

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOrderDetailByProduct(int id)
        {
            try
            {
                var orderDetailToDelete = await _orderDetailService.GetById(id);

                if (orderDetailToDelete != null)
                {
                    var order = await _orderService.GetById(orderDetailToDelete.OrderId);

                    await _orderDetailService.Delete(orderDetailToDelete);

                    var remainingOrderDetails = await _orderDetailService.GetOrderDetailsByOrderId(order.Id);

                    if (remainingOrderDetails == null || !remainingOrderDetails.Any())
                    {
                        await _orderService.Delete(order);
                    }

                    return Ok("Order detail deleted successfully");
                }
                else
                {
                    return BadRequest("Order detail not found.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("An unexpected error occurred.");
            }
        }

        public async Task<IActionResult> ProductCountPerCategory(string SearchText = "", int pg = 1)
        {
            var categories = await _categoryService.GetAll();
            var categoryProductCounts = new List<CategoryProductCountViewModel>();

            if (string.IsNullOrEmpty(SearchText))
            {
                foreach (var category in categories)
                {
                    var productCount = await _productService.GetProductCountByCategoryId(category.Id);

                    categoryProductCounts.Add(new CategoryProductCountViewModel
                    {
                        CategoryId = category.Id,
                        CategoryName = category.Name,
                        ProductQuantity = productCount
                    });
                }
            }
            else
            {
                var filteredCategories = categories
                    .Where(c => c.Name.Unidecode().IndexOf(SearchText.Unidecode(), StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();

                foreach (var category in filteredCategories)
                {
                    var productCount = await _productService.GetProductCountByCategoryId(category.Id);

                    categoryProductCounts.Add(new CategoryProductCountViewModel
                    {
                        CategoryId = category.Id,
                        CategoryName = category.Name,
                        ProductQuantity = productCount
                    });
                }
            }

            const int pageSize = 5;
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = categoryProductCounts.Count();
            int recsSkip = (pg - 1) * pageSize;
            var data = categoryProductCounts.Skip(recsSkip).Take(pageSize).ToList();

            Views.Shared.Components.SearchBar.SPager SearchPager = new Views.Shared.Components.SearchBar.SPager(recsCount, pg, pageSize) { Action = "ProductCountPerCategory", Controller = "Admin", SearchText = SearchText };
            ViewBag.SearchPager = SearchPager;
            Views.Shared.Components.PageDivide.SPager PageDivide = new Views.Shared.Components.PageDivide.SPager(recsCount, pg, pageSize) { Action = "ProductCountPerCategory", Controller = "Admin", SearchText = SearchText };
            ViewBag.PageDivide = PageDivide;

            return View(data);
        }

        public async Task<IActionResult> OrderDetailCountPerProduct(string SearchText = "", int pg = 1)
        {
            var products = await _productService.GetAll();
            var productOrderDetailCounts = new List<ProductOrderDetailCountViewModel>();

            if (string.IsNullOrEmpty(SearchText))
            {
                foreach (var product in products)
                {
                    var orderDetailCount = await _orderDetailService.GetOrderDetailCountByProductId(product.Id);

                    productOrderDetailCounts.Add(new ProductOrderDetailCountViewModel
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        OrderDetailQuantity = orderDetailCount
                    });
                }
            }
            else
            {
                var filteredProducts = products
                    .Where(p => p.Name.Unidecode().IndexOf(SearchText.Unidecode(), StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();

                foreach (var product in filteredProducts)
                {
                    var orderDetailCount = await _orderDetailService.GetOrderDetailCountByProductId(product.Id);

                    productOrderDetailCounts.Add(new ProductOrderDetailCountViewModel
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        OrderDetailQuantity = orderDetailCount
                    });
                }
            }

            const int pageSize = 5;
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = productOrderDetailCounts.Count();
            int recsSkip = (pg - 1) * pageSize;
            var data = productOrderDetailCounts.Skip(recsSkip).Take(pageSize).ToList();

            Views.Shared.Components.SearchBar.SPager SearchPager = new Views.Shared.Components.SearchBar.SPager(recsCount, pg, pageSize) { Action = "OrderDetailCountPerProduct", Controller = "Admin", SearchText = SearchText };
            ViewBag.SearchPager = SearchPager;
            Views.Shared.Components.PageDivide.SPager PageDivide = new Views.Shared.Components.PageDivide.SPager(recsCount, pg, pageSize) { Action = "OrderDetailCountPerProduct", Controller = "Admin", SearchText = SearchText };
            ViewBag.PageDivide = PageDivide;

            return View(data);
        }

        public async Task<IActionResult> OrderDetailCountPerOrder(string SearchText = "", int pg = 1)
        {
            var orders = await _orderService.GetAll();
            var orderOrderDetailCounts = new List<OrderOrderDetailCountViewModel>();

            if (string.IsNullOrEmpty(SearchText))
            {
                foreach (var order in orders)
                {
                    var orderDetailCount = await _orderDetailService.GetOrderDetailCountByOrderId(order.Id);

                    orderOrderDetailCounts.Add(new OrderOrderDetailCountViewModel
                    {
                        OrderId = order.Id,
                        OrderShipName = order.ShipName,
                        OrderTotal = order.Total,
                        OrderDetailQuantity = orderDetailCount
                    });
                }
            }
            else
            {
                var filteredOrders = orders
                    .Where(p => p.ShipName.Unidecode().IndexOf(SearchText.Unidecode(), StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();

                foreach (var order in filteredOrders)
                {
                    var orderDetailCount = await _orderDetailService.GetOrderDetailCountByOrderId(order.Id);

                    orderOrderDetailCounts.Add(new OrderOrderDetailCountViewModel
                    {
                        OrderId = order.Id,
                        OrderShipName = order.ShipName,
                        OrderTotal = order.Total,
                        OrderDetailQuantity = orderDetailCount
                    });
                }
            }

            const int pageSize = 5;
            if (pg < 1)
            {
                pg = 1;
            }
            int recsCount = orderOrderDetailCounts.Count();
            int recsSkip = (pg - 1) * pageSize;
            var data = orderOrderDetailCounts.Skip(recsSkip).Take(pageSize).ToList();

            Views.Shared.Components.SearchBar.SPager SearchPager = new Views.Shared.Components.SearchBar.SPager(recsCount, pg, pageSize) { Action = "OrderDetailCountPerOrder", Controller = "Admin", SearchText = SearchText };
            ViewBag.SearchPager = SearchPager;
            Views.Shared.Components.PageDivide.SPager PageDivide = new Views.Shared.Components.PageDivide.SPager(recsCount, pg, pageSize) { Action = "OrderDetailCountPerOrder", Controller = "Admin", SearchText = SearchText };
            ViewBag.PageDivide = PageDivide;

            return View(data);
        }
    }
}