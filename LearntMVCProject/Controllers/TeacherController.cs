using LearntMVCProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace LearntMVCProject.Controllers
{
    public class TeacherController : Controller
    {
        private UserDbContext db = new UserDbContext();

        public IActionResult TeacherProfile(int id)
        {
            var teacher = db.Users.FirstOrDefault(u => u.Id == id && u.Role=="Teacher");
            if (teacher == null)
            {
                return NotFound(); ;

            }
            var courses = db.Courses.Where(c => c.TeacherId == id).ToList();    
            ViewBag.Courses = courses;
            return View(teacher);
        }
       
        [HttpPost]
        public IActionResult AddCourse(Course course)
        {
            var username = HttpContext.Session.GetString("Username");
            var teacher = db.Users.FirstOrDefault(u => u.Username == username);
            if (teacher !=null && teacher.Role=="Teacher")
            {
                course.TeacherId = teacher.Id;
                db.Courses.Add(course);
                db.SaveChanges();
               
            }
            return RedirectToAction("TeacherProfile", new { id = teacher.Id });
        }
    }
}
