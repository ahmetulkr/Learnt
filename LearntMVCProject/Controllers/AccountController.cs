using LearntMVCProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LearntMVCProject.Models;

namespace LearntMVCProject.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext _context
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {

        }
    }
}
