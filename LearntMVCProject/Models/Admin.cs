using System.ComponentModel.DataAnnotations;

namespace LearntMVCProject.Models
{
    public class Admin
    {
        [Required]
        public int AdminId { get; set; }
        [Required]
        public string AdminUser { get; set; }
        [Required]
        public string AdminPassword { get; set; }
    }
}
