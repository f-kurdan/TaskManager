﻿using System.ComponentModel.DataAnnotations;

namespace TaskManager.ViewModels
{
	public class LoginViewModel
	{
		[Required]
		[Display(Name = "User Name")]
		public string UserName { get; set; }

		[Required]
		public string Password { get; set; }

		[Display(Name = "Remember Me")]
		public bool RememberMe { get; set; }
	}
}
