using LearntMVCProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LearntMVCProject.Controllers
{
    public class LessonsController : Controller
    {
        private readonly ILogger<LessonsController> _logger;

        public LessonsController(ILogger<LessonsController> logger)
        {
            _logger = logger;
        }

        public IActionResult Html()
        {
            return View();
        }

        public IActionResult Css()
        {
            return View();
        }

        public IActionResult Javascript()
        {
            return View();
        }

    }
}
