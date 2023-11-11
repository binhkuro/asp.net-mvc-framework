using System.ComponentModel.DataAnnotations;

namespace FinalEcormer2023.ViewModels {
	public class ForgotPasswordViewModel {
		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		public String Email { get; set; }
	}
}
