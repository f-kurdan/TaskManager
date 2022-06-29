﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index()
        {
            var tasks = _context.Tasks.AsNoTracking().ToList();
            var vm = new TaskViewModel { Tasks = tasks };

            return View(vm);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            var performers = _context.Users
                .AsNoTracking()
                .Select(u => u.ToString())
                .ToList();
            var tags = _context.Tags.AsNoTracking().ToList();
            var statuses = _context.Statuses.AsNoTracking().ToList();

            var vm = new TaskViewModel
            {
                Statuses = statuses,
                Performers = performers,
                Tags = tags
            };
            return View(vm);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(TaskViewModel vm)
        {
            var task = new Models.Task
            {
                Title = vm.Title,
                Author = HttpContext.User.Identity.Name,
                Performer = vm.Performer,
                Created = DateTime.Now,
                Description = vm.Description,
                Status = vm.Status,
                Tags = vm.Tags
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var task = await _context.Tasks.FindAsync(id);
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

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return NotFound();

            _context.Remove(task);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
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
                Tags = task.Tags,
                Statuses = statuses,
                Performers = performers,
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TaskViewModel vm)
        {
            var task = new Models.Task
            {
                ID = vm.ID,
                Title = vm.Title,
                Author = vm.Author,
                Performer = vm.Performer,
                Created = vm.CreationDate,
                Description = vm.Description,
                Status = vm.Status,
                Tags = vm.Tags
            };

            _context.Update(task);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = task.ID });
        }
    }
}
