using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace KataBlogger.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username or Email is required")]
        [MaxLength(20, ErrorMessage = "Max 20 characters allowed")]
        [Display(Name="Username or Email")]
        public string UserNameOrEmail { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, MinimumLength=5, ErrorMessage = "Max 20 or min 5 characters allowed")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

