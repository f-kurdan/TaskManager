using System.ComponentModel.DataAnnotations;

namespace TaskManager.ViewModels.AuthViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"[a-zA-Z\d]{8}",
            ErrorMessage = "Password must be 8 characters long and contain latin letters and numbers")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords don't match")]
        public string ConfirmPassword { get; set; }

        public bool IsEmailConfirmed { get; set; } = false;

        public string ConfirmationString { get; set; } = string.Empty;
    }
}
