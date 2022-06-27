using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
using TaskManager.Data;
using TaskManager.ViewModels;

namespace TaskManager.Controllers
{
	public class AuthController : Controller
	{
		private readonly AppDbContext _context;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly IEmailService _emailService;

		public AuthController(UserManager<IdentityUser> userManager,
			SignInManager<IdentityUser> signInManager,
			IEmailService emailService)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_emailService = emailService;
		}

		[HttpGet]
		public IActionResult Login() => View();

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel vm)
		{
			if (!ModelState.IsValid) return View(vm);

			var user = await _userManager.FindByNameAsync(vm.UserName);
			if (user != null)
			{
				var signInResult = await _signInManager
						.PasswordSignInAsync(user, vm.Password, vm.RememberMe, false);

				if (signInResult.Succeeded)
					return RedirectToAction("Index", "Home");
			}
			return View(vm);
		}

		[Authorize]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public IActionResult Register() => View();

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel vm)
		{
			if (!ModelState.IsValid) return View(vm);

			var user = new IdentityUser
			{
				UserName = vm.UserName,
				Email = vm.Email
			};

			var result = await _userManager.CreateAsync(user, vm.Password);

			if (result.Succeeded)
			{
				var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

				var link = Url.Action(nameof(VerifyEmail), "Auth", new { userId = user.Id, code }, Request.Scheme, Request.Host.ToString());

				await _emailService.SendAsync("test@test.com", "email verification",
					$"<a href=\"{link}\">Click on this link to verify email<a>", true);

				return RedirectToAction("EmailVerification");
			}
			return View(vm);
		}

		public IActionResult EmailVerification() => View();

		public async Task<IActionResult> VerifyEmail(string userId, string code)
		{
			var user = await _userManager.FindByIdAsync(userId);

			if (user == null) return BadRequest();

			var result = await _userManager.ConfirmEmailAsync(user, code);

			if (result.Succeeded)
				return View();
			return BadRequest();
		}
	}
}