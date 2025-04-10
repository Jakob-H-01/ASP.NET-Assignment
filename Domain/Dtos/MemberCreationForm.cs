using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Dtos;

public class MemberCreationForm
{
    public string? Image { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? JobTitle { get; set; }
    public string? Address { get; set; }
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }

    [Column(TypeName = "date")]
    public DateTime? DateOfBirth { get; set; }
    public string? Password { get; set; }
}