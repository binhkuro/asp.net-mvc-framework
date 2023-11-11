using System.ComponentModel.DataAnnotations;

namespace FinalEcormer2023.ViewModels {
	public class LoginViewModel {
		[Required(ErrorMessage ="Please enter your username!")]
		public string Username { get; set; }

		[Required(ErrorMessage ="Please enter your password!")]
		[DataType(DataType.Password)]
		[Display (Name = "Password")]
		public string Password { get; set; }

		[Display(Name ="Remember me")] 
		public bool RememberMe {  get; set; }	
		public string ReturnUrl { get; set; }	
		
	}
}
