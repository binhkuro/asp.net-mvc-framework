using System.ComponentModel.DataAnnotations;

namespace FinalEcormer2023.ViewModels {
	public class ChangePasswordViewModel {

		[Required(ErrorMessage ="Please enter Password")]
		[DataType(DataType.Password)]
		[Display(Name = "oldPassword")]
		public string oldPassword { get; set; }

		[Required(ErrorMessage = "Please enter newPassword")]
		[DataType(DataType.Password)]
		[Display(Name = "newPassword")]
		[StringLength(255, ErrorMessage = "The newPassword  least length: {2}" , MinimumLength = 10)]
		public string newPassword { get; set; }
		[DataType(DataType.Password)]
		[Display(Name = "confirmPassword ")]
		[Compare("newPassword" , ErrorMessage ="Not match")]
		public string confirmPassword {  get; set; }	
		public string? msg { get; set; }	

	}
}
