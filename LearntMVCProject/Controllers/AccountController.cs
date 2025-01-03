using LearntMVCProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LearntMVCProject.Models;
using System.Linq;

namespace LearntMVCProject.Controllers
{
    public class AccountController : Controller
    {   
    
        private UserDbContext db = new UserDbContext();

     

        // Get register
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Register(AppUser user)
        {
            if (ModelState.IsValid)
            {
                db.AddUser(user);
                return RedirectToAction("Login");

            }
           return View(user);

        }
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Login(string username, string password)
{
    var user = db.GetUser(username, password);

    if (user != null)
    {
        HttpContext.Session.SetInt32("UserId", user.Id);
        return RedirectToAction("Profile", "Account");
    }

    ViewBag.Error = "Geçersiz kullanıcı adı veya şifre.";
    return View();
}

public IActionResult Profile()
{
    var userId = HttpContext.Session.GetInt32("UserId");

    if (userId == null)
    {
        // Kullanıcı giriş yapmamışsa sadece bir kez login sayfasına yönlendir.
        if (HttpContext.Request.Path != "/Account/Login")
        {
            return RedirectToAction("Login", "Account");
        }
    }

    var profile = db.GetProfile((int)userId);
    return View(profile);
}

        public ActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}
