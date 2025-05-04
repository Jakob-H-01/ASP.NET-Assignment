using Business.Models;
using Domain.Dtos;
using Domain.Models;

namespace Business.Interfaces;

public interface IProjectService
{
    Task<ServiceResult<bool>> CreateProjectAsync(ProjectCreationForm form);
    Task<ServiceResult<bool>> DeleteProjectAsync(string id);
    Task<ServiceResult<Project>> GetProjectAsync(string id);
    Task<ServiceResult<IEnumerable<Project>>> GetProjectsAsync();
    Task<ServiceResult<bool>> UpdateProjectAsync(ProjectUpdateForm form);
}