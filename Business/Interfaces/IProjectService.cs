using Business.Models;
using Domain.Models;

namespace Business.Interfaces;

public interface IProjectService
{
    Task<ServiceResult<Project>> GetProjectAsync(string id);
    Task<ServiceResult<IEnumerable<Project>>> GetProjectsAsync();
}