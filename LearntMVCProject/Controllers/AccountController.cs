using LearntMVCProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace LearntMVCProject.Controllers
{
    public class AccountController : Controller
    {   
        private readonly UserManager<AppUser> userManager;  
        private readonly SignInManager<AppUser> _signInManager;
         private readonly  AppDbContext _context;
         
        
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]

     public async Task<IActionResult> Login(LoginViewModel model)
     {
         
     }
    }
}
