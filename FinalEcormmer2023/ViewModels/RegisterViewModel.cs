using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace FinalEcormer2023.ViewModels {
	public class RegisterViewModel {
		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		public String Email { get; set; }

		[Required]
		[Display(Name = "UserName")]
		public String UserName {  get; set; }	
		
		[Required]
		[StringLength(25,ErrorMessage ="The {0} must be at least {2} characters long" , MinimumLength = 10)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public String Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name ="Confirm password")]
		[Compare("Password", ErrorMessage ="The message and confirmation password not match")]
		public string ConfirmPassword { get; set;}
		public string? ReturnUrl {  get; set; }

	}
}
