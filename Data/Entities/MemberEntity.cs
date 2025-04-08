using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Data.Entities;

public class MemberEntity : IdentityUser
{
    [ProtectedPersonalData]
    public string? Image { get; set; }

    [ProtectedPersonalData]
    public string FirstName { get; set; } = null!;
    
    [ProtectedPersonalData]
    public string LastName { get; set; } = null!;
    
    [ProtectedPersonalData]
    public string? JobTitle { get; set; }
    
    [ProtectedPersonalData]
    public string? Address { get; set; }
    
    [ProtectedPersonalData]
    [Column(TypeName = "date")]
    public DateTime? DateOfBirth { get; set; }

    public virtual ICollection<ProjectEntity> Projects { get; set; } = [];
}
