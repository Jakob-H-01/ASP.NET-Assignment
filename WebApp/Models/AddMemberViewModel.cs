using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class AddMemberViewModel
{
    public string? Image { get; set; }

    [Display(Name = "First Name", Prompt = "Your first name")]
    [Required(ErrorMessage = "Required")]
    public string FirstName { get; set; } = null!;

    [Display(Name = "Last Name", Prompt = "Your last name")]
    [Required(ErrorMessage = "Required")]
    public string LastName { get; set; } = null!;

    [Display(Name = "Email", Prompt = "Your email address")]
    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "Required")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email")]
    public string Email { get; set; } = null!;

    [Display(Name = "Phone", Prompt = "Your phone number")]
    [DataType(DataType.PhoneNumber)]
    [RegularExpression("^[+]?[0-9]+$", ErrorMessage = "Invalid phone number")]
    public string? PhoneNumber { get; set; }

    [Display(Name = "Job Title", Prompt = "Your job title")]
    public string? JobTitle { get; set; }

    [Display(Name = "Address", Prompt = "Your address")]
    public string? Address { get; set; }

    [Display(Name = "Day", Prompt = "Day")]
    public int? Day { get; set; }

    [Display(Name = "Month", Prompt = "Month")]
    public int? Month { get; set; }

    [Display(Name = "Year", Prompt = "Year")]
    public int? Year { get; set; }

    [Display(Name = "Role")]
    [Required(ErrorMessage = "Required")]
    public string RoleName { get; set; } = null!;
}