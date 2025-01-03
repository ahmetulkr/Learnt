using Microsoft.AspNetCore.Mvc;
using LearntMVCProject.Models;

namespace LearntMVCProject.Controllers
{
    public class UserProfileController : Controller
    {
        private UserDbContext dbContext = new UserDbContext();

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var profile = dbContext.GetProfile((int)userId);
            if (profile == null)
            {
                // Profil yoksa boş bir profil oluştur
                profile = new UserProfile();
            }

            return View(profile);
        }

        [HttpPost]
        public IActionResult UpdateProfile(UserProfile profile)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

            profile.UserId = (int)userId;
            dbContext.AddOrUpdateProfile(profile);

            return RedirectToAction("Profile");
        }
    }
}
