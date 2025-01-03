using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LearntMVCProject.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Bio { get; set; }
        public string ProfilePictureUrl { get; set; }





      

    }
}
