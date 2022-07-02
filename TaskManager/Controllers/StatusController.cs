using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.ViewModels;

namespace TaskManager.Controllers
{
    public class StatusController : Controller
    {
        private readonly AppDbContext _context;

        public StatusController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var statuses = await _context.Statuses.AsNoTracking().ToListAsync();
            return View(new StatusViewModel { Statuses = statuses });
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create() => View();

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StatusViewModel vm)
        {
            var status = new Status
            {
                Title = vm.Title,
                Created = DateTime.Now
            };

            _context.Statuses.Add(status);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var status = await _context.Statuses.FindAsync(id);
            if (status == null) return NotFound();

            var vm = new StatusViewModel
            {
                ID = status.ID,
                Title = status.Title,
                Created = status.Created
            };

            return View(vm);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(StatusViewModel vm)
        {
            var status = new Status
            {
                ID = vm.ID,
                Title = vm.Title,
                Created = vm.Created
            };

            _context.Statuses.Update(status);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var status = await _context.Statuses.FindAsync(id);
            if (status == null) return NotFound();

            _context.Statuses.Remove(status);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
