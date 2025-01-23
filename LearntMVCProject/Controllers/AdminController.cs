using LearntMVCProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace LearntMVCProject.Controllers
{
    public class AdminController : Controller
    {
        private UserDbContext dbContext = new UserDbContext();
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(Admin admin)
        {
         var user = dbContext.GetAdmin(admin.AdminUser, admin.AdminPassword);
                if (user != null)
                {
                    HttpContext.Session.SetInt32("AdminId", user.AdminId);
                    HttpContext.Session.SetString("AdminUser", user.AdminUser);
                    return RedirectToAction("Login", "Admin"); 
                }
                ViewBag.Error = "Geçersiz kullanıcı adı veya şifre. Lütfen tekrar deneyin.";
          return View(); 
        }
        public IActionResult Page()
        {
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId == null)
            {
                return RedirectToAction("Login", "Admin");
            }

            // Pass the adminId to the view
            ViewBag.AdminId = adminId;
            return View("AdminPage"); // Ensure you have an AdminPage.cshtml view
        }

        
    }
}
