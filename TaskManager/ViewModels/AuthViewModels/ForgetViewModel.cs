using System.ComponentModel.DataAnnotations;

namespace TaskManager.ViewModels.AuthViewModels
{
    public class ForgetViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
