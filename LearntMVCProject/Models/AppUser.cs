using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LearntMVCProject.Models
{
    public class AppUser
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

      

    }
}
