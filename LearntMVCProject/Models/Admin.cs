using System.ComponentModel.DataAnnotations;

namespace LearntMVCProject.Models
{
    public class Admin
    {
        public int AdminId { get; set; }
        public string AdminUser { get; set; }
        public string AdminPassword { get; set; }
        
    }
}
