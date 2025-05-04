using Business.Interfaces;
using Business.Models;
using Data.Interfaces;
using Domain.Extensions;
using Domain.Models;

namespace Business.Services;

public class ProjectService(IProjectRepository projectRepository) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;

    public async Task<ServiceResult<IEnumerable<Project>>> GetProjectsAsync()
    {
        var result = await _projectRepository.GetAllAsync
            (
                orderByDescending: false,
                sortBy: null,
                filterBy: null,
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
}