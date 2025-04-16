using Domain.Models;

namespace WebApp.Models;

public class TeamMembersViewModel
{
    public IEnumerable<Member> Members { get; set; } = [];
    public AddMemberViewModel AddMember { get; set; } = new();
    public EditMemberViewModel EditMember { get; set; } = new();
    public bool ShowAddModal { get; set; } = false;
    public bool ShowEditModal { get; set; } = false;
}