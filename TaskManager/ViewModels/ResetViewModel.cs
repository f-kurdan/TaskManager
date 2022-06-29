using System.ComponentModel.DataAnnotations;

namespace TaskManager.ViewModels
{
	public class ResetViewModel
	{
		public string UserID { get; set; }

		public string Email { get; set; }
		public string Token { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Passwords don't match.")]
		public string ConfirmPassword { get; set; }
	}
}
