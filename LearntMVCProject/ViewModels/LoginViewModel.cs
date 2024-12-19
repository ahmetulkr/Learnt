using System.ComponentModel.DataAnnotations;

namespace LearntMVCProject.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name="Email Address")]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }   
}
