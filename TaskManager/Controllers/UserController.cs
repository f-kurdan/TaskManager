using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.ViewModels;

namespace TaskManager.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(bool deleteButtonPressed)
        {
            var users = _context.Users.AsNoTracking().ToList();
            if (deleteButtonPressed)
            {
                return View(new UserViewModel { Users = users, TriedToDeleteUser = true });
            }
            return View(new UserViewModel { Users = users });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return BadRequest();
            return RedirectToAction(nameof(ConfirmDeletion), new { identityUser = user });
        }

        private async Task<IActionResult> ConfirmDeletion(IdentityUser identityUser)
        {
            _context.Users.Remove(identityUser);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
