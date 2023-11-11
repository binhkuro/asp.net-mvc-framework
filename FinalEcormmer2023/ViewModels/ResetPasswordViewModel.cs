using System.ComponentModel.DataAnnotations;

namespace FinalEcormer2023.ViewModels {
	public class ResetPasswordViewModel {
		[Required]
		[EmailAddress(ErrorMessage ="Email not valid")]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[StringLength(255, ErrorMessage = "The newPassword  least length: {2}", MinimumLength = 10)]
		[Display(Name = "newPassword")]
		public string newPassword { get; set; }
		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Confirm Pass")]
		[Compare("newPassword", ErrorMessage = "Not match")]
		public string ConfirmPassword { get; set;}
		public string? StatusCode { get; set; }
	}
}
