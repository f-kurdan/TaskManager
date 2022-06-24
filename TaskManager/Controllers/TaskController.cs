using Microsoft.AspNetCore.Mvc;
using TaskManager.ViewModels;

namespace TaskManager.Controllers
{
    public class TaskController : Controller
    {
        public IActionResult Index() => View();

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(TaskViewModel vm)
        {
            return View();
        }

    }



}
