using System.ComponentModel.DataAnnotations;

namespace KataBlog.ViewModels
{
    public class RegisterVM
    {
        [Display(Name = "First name:")]
        [Required(ErrorMessage = "First name is required")]
        [MaxLength(50, ErrorMessage = "Max 50 characters allowed")]
        public string? FirstName { get; set; }

        [Display(Name = "Last name:")]
        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(50, ErrorMessage = "Max 50 characters allowed")]
        public string? LastName { get; set; }

        [Display(Name = "Email address:")]
        [Required(ErrorMessage="Email is required")]
        [MaxLength(100, ErrorMessage = "Max 100 characters allowed")]
        // [EmailAddress(ErrorMessage = "Please Enter Valid Email.")]
        [RegularExpression(@"^([\w-\.]+)@((\[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please Enter Valid Email.")]
        [EmailAddress]
        public string? Email { get; set; }

        [Display(Name = "Username:")]
        [Required(ErrorMessage = "Username is required")]
        [MaxLength(20, ErrorMessage = "Max 20 characters allowed")]
        public string? UserName { get; set; }

        [Display(Name = "Password:")]
        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, MinimumLength=5, ErrorMessage = "Max 20 or min 5 characters allowed")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        public bool IsAdmin { get; set; }
    }
}
