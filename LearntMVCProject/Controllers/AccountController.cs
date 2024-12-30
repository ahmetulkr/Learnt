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
        public ActionResult Login(AppUser user)
        {
            var loginUser = db.GetUser(user.Username, user.Password);
            if (loginUser != null)
            {
                HttpContext.Session.SetString("Username", loginUser.Username);
                HttpContext.Session.SetString("Role", loginUser.Role);
                if (loginUser.Role=="Teacher")
                {
                    return RedirectToAction("TeacherProfile", "Teacher", new { id = loginUser.Id });
                }
                else if (loginUser.Role== "Student")
                {
                    return RedirectToAction("StudentProfile", "Student", new { id = loginUser.Id });
                }
                
            }
            ViewBag.Error = "Invalid Username or Password";
            return View(user);
        }
        public ActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}
