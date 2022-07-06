using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.ViewModels;

namespace TaskManager.Controllers
{
    public class TagController : Controller
    {
        private readonly AppDbContext _context;

        public TagController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var tags = await _context.Tags.AsNoTracking().ToListAsync();
            return View(new TagViewModel { Tags = tags });
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(TagViewModel vm)
        {
            var tag = new Tag
            {
                Title = vm.Title,
                Created = DateTime.Now,
            };

            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var tag = await _context.Tags.FindAsync(id);
            if (tag == null) return NotFound();

            var vm = new TagViewModel
            {
                ID = tag.ID,
                Title = tag.Title,
                Created = tag.Created
            };

            return View(vm);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(TagViewModel vm)
        {
            var tag = new Tag
            {
                ID = vm.ID,
                Title = vm.Title,
                Created = vm.Created
            };

            _context.Tags.Update(tag);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var tag = await _context.Tags.FindAsync(id);

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
