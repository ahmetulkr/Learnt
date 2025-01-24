using LearntMVCProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace LearntMVCProject.Controllers
{
    public class AccountController : Controller
    {
        private UserDbContext db = new UserDbContext();

        // Kayıt formunu göster
        public ActionResult Register()
        {
            return View();
        }

        // Kayıt işlemi
        [HttpPost]
        public ActionResult Register(AppUser user)
        {
            if (ModelState.IsValid)
            {
                db.AddUser(user);

                // Kullanıcı için varsayılan profil oluştur
                var createdUser = db.GetUser(user.Username, user.Password);
                if (createdUser != null)
                {
                    db.CreateDefaultProfile(createdUser.Id);
                }

                return RedirectToAction("Login");
            }
            return View(user);
        }

        // Giriş formunu göster
        public IActionResult Login()
        {
            return View();
        }

        // Giriş işlemi
        [HttpPost]
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = db.GetUser(username, password); // Kullanıcıyı veritabanından al

            if (user != null)
            {
                // Kullanıcı bilgilerini oturuma kaydet
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", user.Role); // Kullanıcı rolünü sakla

                // Rolüne göre yönlendir
                if (user.Role == "admin")
                {
                    return RedirectToAction("AdminPage", "Admin");
                }

                // Normal kullanıcı için yönlendirme
                return RedirectToAction("Home", "Home");
            }

            // Hatalı giriş mesajı
            ViewBag.Error = "Geçersiz kullanıcı adı veya şifre.";
            return View();
        }

        // Profil sayfası
        public IActionResult Profile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var profile = db.GetProfile((int)userId);
            
            // Profil yoksa otomatik oluştur
            if (profile == null)
            {
                db.CreateDefaultProfile((int)userId);
                profile = db.GetProfile((int)userId);
            }

            return View(profile);
        }

        // Profil düzenleme sayfası
        public IActionResult EditProfile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login");
            }

            var profile = db.GetProfile((int)userId);

            // Profil yoksa oluştur
            if (profile == null)
            {
                db.CreateDefaultProfile((int)userId);
                profile = db.GetProfile((int)userId);
            }

            return View(profile);
        }

        // Profil düzenleme işlemi
        [HttpPost]
        
        
public IActionResult EditProfile(UserProfile profile)
{
    var userId = HttpContext.Session.GetInt32("UserId");

    if (userId == null)
    {
        return RedirectToAction("Login");
    }

    // UserId'yi güvenli bir şekilde ata
    profile.UserId = (int)userId;

    if (ModelState.IsValid)
    {
        db.AddOrUpdateProfile(profile);
        return RedirectToAction("Profile");
    }

    if (profile.UserId == 0)
{
    profile.UserId = (int)HttpContext.Session.GetInt32("UserId");
}


    // Model hatası varsa aynı sayfaya döner
    return View(profile);
}


        // Oturumu kapat
        public ActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        public IActionResult LessonLogin()
        {
            return View();
        }
        [HttpPost]
        public IActionResult LessonLogin(string username, string password, string course)
        {
            var user = db.GetUser(username, password);

            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.Id);


                return RedirectToAction("Home", "Home");
            }

            ViewBag.Error = "Invalid username or password.";
            return View();
        }

        public IActionResult AdminPage()
        {
            // Oturumdan kullanıcı rolünü al
            var userRole = HttpContext.Session.GetString("UserRole");

            // Eğer kullanıcı admin değilse giriş sayfasına yönlendir
            if (userRole != "Admin")
            {
                return RedirectToAction("Login", "User");
            }

            // Admin sayfasını döndür
            return View();
        }
    }
}
