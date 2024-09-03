using System.ComponentModel.DataAnnotations;
using Microsoft.Build.Framework;

namespace KataBlog.ViewModels
{
    public class LoginVM
    {
        
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Username or Email is required")]
        [MaxLength(20, ErrorMessage = "Max 20 characters allowed")]
        public string? Username { get; set; }
        
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "Password is required")]
        [StringLength(20, MinimumLength=5, ErrorMessage = "Max 20 or min 5 characters allowed")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
