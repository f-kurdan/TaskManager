﻿using System.ComponentModel.DataAnnotations;

namespace TaskManager.ViewModels.AuthViewModels
{
    public class ResetViewModel
    {
        public string UserID { get; set; }
        public string Token { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords don't match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        public bool IsPasswordReset { get; set; } 
    }
}
