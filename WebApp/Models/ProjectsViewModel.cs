using Domain.Models;

namespace WebApp.Models;

public class ProjectsViewModel
{
    public IEnumerable<Project> Projects { get; set; } = [];
    public AddProjectViewModel AddProject { get; set; } = new();
    public EditProjectViewModel EditProject { get; set; } = new();
    public bool ShowAddModal { get; set; } = false;
    public bool ShowEditModal { get; set; } = false;
}