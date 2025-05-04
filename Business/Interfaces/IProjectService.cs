using System.Linq.Expressions;
using Business.Models;
using Data.Entities;
using Domain.Dtos;
using Domain.Models;

namespace Business.Interfaces;

public interface IProjectService
{
    Task<ServiceResult<bool>> CreateProjectAsync(ProjectCreationForm form);
    Task<ServiceResult<bool>> DeleteProjectAsync(string id);
    Task<ServiceResult<Project>> GetProjectAsync(string id);
    Task<ServiceResult<IEnumerable<Project>>> GetProjectsAsync(Expression<Func<ProjectEntity, bool>>? filterBy = null);
    Task<ServiceResult<bool>> UpdateProjectAsync(ProjectUpdateForm form);
}