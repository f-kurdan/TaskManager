using System.Security.Claims;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
using TaskManager.Data;
using TaskManager.ViewModels.AuthViewModels;

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
                    return RedirectToAction(nameof(Index), "Home");
            }
            vm.SignInFailed = true;
            return View(vm);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Index), "Home");
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
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callback = Url.Action(nameof(VerifyEmail), "Auth", new { userId = user.Id, token }, Request.Scheme, Request.Host.ToString());
                //await _emailService.SendAsync(vm.Email, "email verification",
                //    $"<a href=\"{callback}\">Click on this link to verify email<a>", true);
                vm.IsEmailConfirmed = true;
                vm.ConfirmationString = callback;
            }
            return View(vm);
        }

        public async Task<IActionResult> VerifyEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return BadRequest();

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
                return RedirectToAction(nameof(Login));
            return BadRequest();
        }

        [HttpGet]
        public IActionResult ForgetPassword() => View();

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetViewModel vm)
        {
            var user = await _userManager.FindByEmailAsync(vm.Email);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callback = Url.Action(nameof(ResetPassword), "Auth", new { userId = user.Id, token }, Request.Scheme, Request.Host.ToString());


                //await _emailService.SendAsync(vm.Email, "Password reset",
                //    $"<a href=\"{callback}\">Click on this link to create new password<a>", true);

                //vm.IsEmailSend = true;
                vm.EmailResetLink = callback;
                vm.Email = null;
                return View(vm);
            }
            vm.IsUserVerified = false;
            return (View(vm));
        }

        [HttpGet]
        public IActionResult ResetPassword(string userId, string token)
        {
            var vm = new ResetViewModel { UserID = userId, Token = token };
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetViewModel vm)
        {
            if (!ModelState.IsValid) return View();

            var user = await _userManager.FindByIdAsync(vm.UserID);
            if (user == null)
                return BadRequest();

            var result = await _userManager.ResetPasswordAsync(user, vm.Token, vm.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return View();
            }

            vm.IsPasswordReset = true;
            return View(vm);
        }
    }
}