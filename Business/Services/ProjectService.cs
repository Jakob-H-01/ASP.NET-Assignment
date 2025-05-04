using System.Linq.Expressions;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Domain.Dtos;
using Domain.Extensions;
using Domain.Models;

namespace Business.Services;

public class ProjectService(IProjectRepository projectRepository, IStatusService statusService) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IStatusService _statusService = statusService;

    public async Task<ServiceResult<bool>> CreateProjectAsync(ProjectCreationForm form)
    {
        if (form == null)
            return new ServiceResult<bool> { Succeeded = false, StatusCode = 400, Error = "Form can't be null" };

        var entity = form.MapTo<ProjectEntity>();

        var statusResult = await _statusService.GetStatusByNameAsync("Started");
        var status = statusResult.Result;

        entity.StatusId = status!.Id;

        var result = await _projectRepository.AddAsync(entity);
        return result.MapTo<ServiceResult<bool>>();
    }

    public async Task<ServiceResult<IEnumerable<Project>>> GetProjectsAsync(Expression<Func<ProjectEntity, bool>>? filterBy = null)
    {
        var result = await _projectRepository.GetAllAsync
            (
                orderByDescending: false,
                sortBy: null,
                filterBy: filterBy,
                include => include.Member,
                include => include.Status,
                include => include.Client
            );

        return result.MapTo<ServiceResult<IEnumerable<Project>>>();
    }

    public async Task<ServiceResult<Project>> GetProjectAsync(string id)
    {
        var result = await _projectRepository.GetAsync
            (
                findBy: x => x.Id == id,
                include => include.Member,
                include => include.Status,
                include => include.Client
            );

        return result.MapTo<ServiceResult<Project>>();
    }

    public async Task<ServiceResult<bool>> UpdateProjectAsync(ProjectUpdateForm form)
    {
        if (form == null)
            return new ServiceResult<bool> { Succeeded = false, StatusCode = 400, Error = "Form can't be null" };

        var projectResult = await _projectRepository.GetEntityAsync(x => x.Id == form.Id);
        if (!projectResult.Succeeded)
            return new ServiceResult<bool> { Succeeded = false, StatusCode = 404, Error = "Project doesn't exist" };

        var entity = projectResult.Result;
        entity.MapFrom(form);

        var result = await _projectRepository.UpdateAsync(entity!);
        return result.MapTo<ServiceResult<bool>>();
    }

    public async Task<ServiceResult<bool>> DeleteProjectAsync(string id)
    {
        var result = await _projectRepository.DeleteAsync(x => x.Id == id);
        return result.MapTo<ServiceResult<bool>>();
    }
}