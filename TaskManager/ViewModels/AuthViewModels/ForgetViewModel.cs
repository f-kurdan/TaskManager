using System.ComponentModel.DataAnnotations;

namespace TaskManager.ViewModels.AuthViewModels
{
    public class ForgetViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public bool IsEmailSend { get; set; } = false;

        public bool IsUserVerified { get; set; } = true;
    }
}
