using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class LoginViewModel
{
    [Display(Name = "Email", Prompt = "Your email address")]
    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "Required")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email")]
    public string Email { get; set; } = null!;

    [Display(Name = "Password", Prompt = "Enter your password")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Required")]
    [RegularExpression(".{8,}", ErrorMessage = "Minimum 8 characters")]
    public string Password { get; set; } = null!;

    [Display(Name = "Remember Me")]
    public bool RememberMe { get; set; }
}
