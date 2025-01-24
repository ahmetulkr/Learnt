using LearntMVCProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace LearntMVCProject.Controllers
{
    public class AdminController : Controller
    {
        //private UserDbContext db = new UserDbContext();
        //public IActionResult AdminLogin()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult AdminLogin(string adminUser, string adminPassword)
        //{
        //    // Veritabanından admin bilgilerini kontrol ediyoruz
        //    var user = db.GetAdmin(adminUser, adminPassword);

        //    if (user != null)
        //    {
        //        // Kullanıcı bilgilerini oturuma kaydediyoruz
        //        HttpContext.Session.SetInt32("AdminId", user.AdminId);       // Admin ID
        //        HttpContext.Session.SetString("AdminUser", user.AdminUser); // Admin Kullanıcı Adı


        //        return RedirectToAction("AdminPage", "Admin");
        //    }

        //    // Kullanıcı bilgileri doğru değilse hata mesajı gösteriyoruz
        //    ViewBag.Error = "Geçersiz kullanıcı adı veya şifre. Lütfen tekrar deneyin.";
        //    return View("AdminLogin"); // Giriş sayfasını yeniden göster
        //}



        public IActionResult AdminPage()
        {
            // Oturumdan kullanıcı rolünü al
            var userRole = HttpContext.Session.GetString("Role");

            // Eğer kullanıcı admin değilse giriş sayfasına yönlendir
            if (userRole != "admin")
            {
                return RedirectToAction("Home", "Home");
            }

            // Admin sayfasını döndür
            return View();
        }


    }
}
