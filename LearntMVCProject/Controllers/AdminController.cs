using LearntMVCProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace LearntMVCProject.Controllers
{
    public class AdminController : Controller
    {
        private UserDbContext db = new UserDbContext();
        
        public IActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AdminLogin(string adminUser, string adminPassword)
        {
            // Normal Users tablosundan admin rolüne sahip kullanıcıyı kontrol ediyoruz
            var user = db.GetUser(adminUser, adminPassword);

            if (user != null && user.Role == "admin")
            {
                // Kullanıcı bilgilerini oturuma kaydediyoruz
                HttpContext.Session.SetInt32("AdminId", user.Id);       // Admin ID
                HttpContext.Session.SetString("AdminUser", user.Username); // Admin Kullanıcı Adı
                HttpContext.Session.SetString("Role", user.Role); // Admin rolü

                return RedirectToAction("AdminPage", "Admin");
            }

            // Kullanıcı bilgileri doğru değilse veya admin rolü yoksa hata mesajı gösteriyoruz
            ViewBag.Error = "Geçersiz kullanıcı adı veya şifre ya da admin yetkiniz yok. Lütfen tekrar deneyin.";
            return View("AdminLogin"); // Giriş sayfasını yeniden göster
        }

        public IActionResult AdminPage()
        {
            // Oturumdan kullanıcı rolünü al
            var userRole = HttpContext.Session.GetString("Role");

            // Eğer kullanıcı admin değilse giriş sayfasına yönlendir
            if (userRole != "admin")
            {
                return RedirectToAction("Home", "Home");
            }

            // Tüm kursları getir
            var courses = db.GetAllCourses();
            
            // Admin sayfasını döndür
            return View(courses);
        }
        
        // Kurs ekleme sayfasını göster
        public IActionResult AddCourse()
        {
            // Oturumdan kullanıcı rolünü al
            var userRole = HttpContext.Session.GetString("Role");

            // Eğer kullanıcı admin değilse giriş sayfasına yönlendir
            if (userRole != "admin")
            {
                return RedirectToAction("Home", "Home");
            }
            
            return View();
        }
        
        // Kurs ekleme işlemi
        [HttpPost]
        public IActionResult AddCourse(Course course)
        {
            // Oturumdan kullanıcı rolünü al
            var userRole = HttpContext.Session.GetString("Role");

            // Eğer kullanıcı admin değilse giriş sayfasına yönlendir
            if (userRole != "admin")
            {
                return RedirectToAction("Home", "Home");
            }
            
            // Admin ID'sini oturumdan al
            var adminId = HttpContext.Session.GetInt32("AdminId");
            if (adminId.HasValue)
            {
                course.AdminId = adminId.Value;
                
                // Kursu veritabanına ekle
                db.AddCourse(course);
                
                // Admin sayfasına yönlendir
                return RedirectToAction("AdminPage");
            }
            
            return View(course);
        }
        
        // Kurs düzenleme sayfasını göster
        public IActionResult EditCourse(int id)
        {
            // Oturumdan kullanıcı rolünü al
            var userRole = HttpContext.Session.GetString("Role");

            // Eğer kullanıcı admin değilse giriş sayfasına yönlendir
            if (userRole != "admin")
            {
                return RedirectToAction("Home", "Home");
            }
            
            // Kursu veritabanından getir
            var course = db.GetCourseById(id);
            if (course == null)
            {
                return NotFound();
            }
            
            return View(course);
        }
        
        // Kurs düzenleme işlemi
        [HttpPost]
        public IActionResult EditCourse(Course course)
        {
            // Oturumdan kullanıcı rolünü al
            var userRole = HttpContext.Session.GetString("Role");

            // Eğer kullanıcı admin değilse giriş sayfasına yönlendir
            if (userRole != "admin")
            {
                return RedirectToAction("Home", "Home");
            }
            
            // Kursu güncelle
            db.UpdateCourse(course);
            
            // Admin sayfasına yönlendir
            return RedirectToAction("AdminPage");
        }
        
        // Kurs silme işlemi
        public IActionResult DeleteCourse(int id)
        {
            // Oturumdan kullanıcı rolünü al
            var userRole = HttpContext.Session.GetString("Role");

            // Eğer kullanıcı admin değilse giriş sayfasına yönlendir
            if (userRole != "admin")
            {
                return RedirectToAction("Home", "Home");
            }
            
            // Kursu sil
            db.DeleteCourse(id);
            
            // Admin sayfasına yönlendir
            return RedirectToAction("AdminPage");
        }
        
        // Kurs detayı sayfasını göster
        public IActionResult CourseDetails(int id)
        {
            // Oturumdan kullanıcı rolünü al
            var userRole = HttpContext.Session.GetString("Role");

            // Eğer kullanıcı admin değilse giriş sayfasına yönlendir
            if (userRole != "admin")
            {
                return RedirectToAction("Home", "Home");
            }
            
            // Kursu veritabanından getir
            var course = db.GetCourseById(id);
            if (course == null)
            {
                return NotFound();
            }
            
            // Kursa ait videoları getir
            course.VideoLessons = db.GetVideoLessonsByCourseId(id);
            
            return View(course);
        }
        
        // Video ekleme sayfasını göster
        public IActionResult AddVideo(int courseId)
        {
            // Oturumdan kullanıcı rolünü al
            var userRole = HttpContext.Session.GetString("Role");

            // Eğer kullanıcı admin değilse giriş sayfasına yönlendir
            if (userRole != "admin")
            {
                return RedirectToAction("Home", "Home");
            }
            
            // Kursu veritabanından getir
            var course = db.GetCourseById(courseId);
            if (course == null)
            {
                return NotFound();
            }
            
            // Yeni bir video nesnesi oluştur
            var video = new VideoLesson
            {
                CourseId = courseId,
                OrderIndex = db.GetVideoLessonsByCourseId(courseId).Count + 1 // Sıradaki index
            };
            
            ViewBag.CourseName = course.Name;
            
            return View(video);
        }
        
        // Video ekleme işlemi
        [HttpPost]
        public IActionResult AddVideo(VideoLesson video)
        {
            // Oturumdan kullanıcı rolünü al
            var userRole = HttpContext.Session.GetString("Role");

            // Eğer kullanıcı admin değilse giriş sayfasına yönlendir
            if (userRole != "admin")
            {
                return RedirectToAction("Home", "Home");
            }
            
            // Videoyu veritabanına ekle
            db.AddVideoLesson(video);
            
            // Kurs detayı sayfasına yönlendir
            return RedirectToAction("CourseDetails", new { id = video.CourseId });
        }
        
        // Video düzenleme sayfasını göster
        public IActionResult EditVideo(int id)
        {
            // Oturumdan kullanıcı rolünü al
            var userRole = HttpContext.Session.GetString("Role");

            // Eğer kullanıcı admin değilse giriş sayfasına yönlendir
            if (userRole != "admin")
            {
                return RedirectToAction("Home", "Home");
            }
            
            // Videoyu veritabanından getir
            var video = db.GetVideoLessonById(id);
            if (video == null)
            {
                return NotFound();
            }
            
            // Kursu veritabanından getir
            var course = db.GetCourseById(video.CourseId);
            ViewBag.CourseName = course.Name;
            
            return View(video);
        }
        
        // Video düzenleme işlemi
        [HttpPost]
        public IActionResult EditVideo(VideoLesson video)
        {
            // Oturumdan kullanıcı rolünü al
            var userRole = HttpContext.Session.GetString("Role");

            // Eğer kullanıcı admin değilse giriş sayfasına yönlendir
            if (userRole != "admin")
            {
                return RedirectToAction("Home", "Home");
            }
            
            // Videoyu güncelle
            db.UpdateVideoLesson(video);
            
            // Kurs detayı sayfasına yönlendir
            return RedirectToAction("CourseDetails", new { id = video.CourseId });
        }
        
        // Video silme işlemi
        public IActionResult DeleteVideo(int id)
        {
            // Oturumdan kullanıcı rolünü al
            var userRole = HttpContext.Session.GetString("Role");

            // Eğer kullanıcı admin değilse giriş sayfasına yönlendir
            if (userRole != "admin")
            {
                return RedirectToAction("Home", "Home");
            }
            
            // Videoyu veritabanından getir
            var video = db.GetVideoLessonById(id);
            if (video == null)
            {
                return NotFound();
            }
            
            // Videoyu sil
            db.DeleteVideoLesson(id);
            
            // Kurs detayı sayfasına yönlendir
            return RedirectToAction("CourseDetails", new { id = video.CourseId });
        }
    }
}
