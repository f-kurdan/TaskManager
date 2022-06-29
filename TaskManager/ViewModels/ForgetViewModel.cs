using System.ComponentModel.DataAnnotations;

namespace TaskManager.ViewModels
{
	public class ForgetViewModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
	}
}
