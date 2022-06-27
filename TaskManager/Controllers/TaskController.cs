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

            return RedirectToAction("Index");
        }
    }
}
