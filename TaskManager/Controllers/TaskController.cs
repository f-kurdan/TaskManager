using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;
using TaskManager.ViewModels;

namespace TaskManager.Controllers
{
    public class TaskController : Controller
    {
        private readonly AppDbContext _context;

        public TaskController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(TaskViewModel vm)
        {
            var performers = await _context.Users
                    .AsNoTracking()
                    .Select(u => u.ToString())
                    .ToListAsync();
            var tags = await _context.Tags.AsNoTracking().ToListAsync();
            var statuses = await _context.Statuses.AsNoTracking().ToListAsync();
            var tasks = GetFilteredTasksList(vm);

            return View(new TaskViewModel
            {
                Tasks = tasks.ToList(),
                Performers = performers,
                Statuses = statuses,
                Tags = tags
            });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var performers = await _context.Users
                .AsNoTracking()
                .Select(u => u.ToString())
                .ToListAsync();
            var tags = await _context.Tags.AsNoTracking().ToListAsync();
            var statuses = await _context.Statuses.AsNoTracking().ToListAsync();

            var vm = new TaskViewModel
            {
                Statuses = statuses,
                Performers = performers,
                TagSelectList = tags.Select(t => new SelectListItem
                {
                    Text = t.Title,
                    Value = t.ID.ToString()
                }).AsQueryable()
            };
            return View(vm);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(TaskViewModel vm)
        {
            var tags = new List<Tag>();
            if (vm.TagIDs != null)
                tags = _context.Tags
                    .Where(t => vm.TagIDs.Contains(t.ID))
                    .ToList();

            var task = new Models.Task
            {
                Title = vm.Title,
                Author = HttpContext.User.Identity.Name,
                Performer = vm.Performer,
                Created = DateTime.Now,
                Description = vm.Description,
                Status = vm.Status,
                Tags = tags
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var task = await _context.Tasks
                .Include(t => t.Tags)
                .Where(i => i.ID == id)
                .SingleAsync();
            if (task == null) return NotFound();

            var vm = new TaskViewModel
            {
                ID = task.ID,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                Author = task.Author,
                Performer = task.Performer,
                CreationDate = task.Created,
                Tags = task.Tags
            };

            return View(vm);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var task = _context.Tasks
                .Include(task => task.Tags)
                .FirstOrDefault(t => t.ID == id);
            if (task == null) return NotFound();

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return NotFound();

            var performers = _context.Users
                .AsNoTracking()
                .Select(u => u.ToString())
                .ToList();
            var tags = _context.Tags.AsNoTracking().ToList();
            var statuses = _context.Statuses.AsNoTracking().ToList();

            var vm = new TaskViewModel
            {
                ID = task.ID,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                Author = task.Author,
                Performer = task.Performer,
                CreationDate = task.Created,
                TagSelectList = tags.Select(t => new SelectListItem
                {
                    Text = t.Title,
                    Value = t.ID.ToString()
                }).AsQueryable(),
                Statuses = statuses,
                Performers = performers,
            };

            return View(vm);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(TaskViewModel vm)
        {
            var tags = new List<Tag>();
            if (vm.TagIDs != null)
                tags = _context.Tags
                    .Where(t => vm.TagIDs.Contains(t.ID))
                    .ToList();

            var task = new Models.Task
            {
                ID = vm.ID,
                Title = vm.Title,
                Author = vm.Author,
                Performer = vm.Performer,
                Created = vm.CreationDate,
                Description = vm.Description,
                Status = vm.Status,
                Tags = tags
            };

            foreach (var tag in _context.Tags.Where(t => t.TaskID == task.ID))
            {
                tag.TaskID = null;
            }
            _context.Update(task);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = task.ID });
        }

        private IQueryable<Models.Task> GetFilteredTasksList(TaskViewModel vm)
        {
            var tasks = _context.Tasks.Include(t => t.Tags).AsNoTracking();
            if (vm.Performer != null)
                tasks = tasks
                    .Where(t => t.Performer == vm.Performer);
            if (vm.Status != null)
                tasks = tasks
                    .Where(t => t.Status == vm.Status);
            if (vm.Tag != null)
                tasks = tasks
                    .Where(task => task.Tags.Any(tag => tag.Title == vm.Tag));
            if (vm.OnlyCurrentUsersTasks)
                tasks = tasks
                    .Where(t => t.Performer == HttpContext.User.Identity.Name);
            return tasks;
        }
    }
}
