using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Controllers
{
    public class TaskController : Controller
    {
        public IActionResult Index() => View();

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(TaskViewModel vm)

    }

   
    
}
