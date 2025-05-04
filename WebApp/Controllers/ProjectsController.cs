using Business.Interfaces;
using Domain.Dtos;
using Domain.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;

namespace WebApp.Controllers;

[Authorize]
[Route("projects")]
public class ProjectsController(IProjectService projectService, IClientService clientService) : Controller
{
    private readonly IProjectService _projectService = projectService;
    private readonly IClientService _clientService = clientService;

    public async Task<IActionResult> Projects()
    {
        var result = await _projectService.GetProjectsAsync();
        var projects = result.Result;

        var model = new ProjectsViewModel
        {
            Projects = projects!
        };

        return View(model);
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> AddProject([Bind(Prefix = "AddProject")] AddProjectViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var result = await _projectService.GetProjectsAsync();
            var projects = result.Result;
            var viewModel = new ProjectsViewModel
            {
                Projects = projects!,
                AddProject = model,
                ShowAddModal = true
            };

            return View("Projects", viewModel);
        }

        var projectForm = model.MapTo<ProjectCreationForm>();

        var clientResult = await _clientService.GetClientByNameAsync(model.ClientName);
        if (!clientResult.Succeeded)
        {
            await _clientService.CreateClientAsync(model.ClientName);
            clientResult = await _clientService.GetClientByNameAsync(model.ClientName);
        }

        projectForm.ClientId = clientResult.Result!.Id;

        await _projectService.CreateProjectAsync(projectForm);

        return RedirectToAction("Projects");
    }

    [HttpPost]
    [Route("edit")]
    public async Task<IActionResult> EditProject([Bind(Prefix = "EditProject")] EditProjectViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var result = await _projectService.GetProjectsAsync();
            var projects = result.Result;
            var viewModel = new ProjectsViewModel
            {
                Projects = projects!,
                EditProject = model,
                ShowEditModal = true
            };

            return View("Projects", viewModel);
        }

        var projectForm = model.MapTo<ProjectUpdateForm>();

        var clientResult = await _clientService.GetClientByNameAsync(model.ClientName);
        if (!clientResult.Succeeded)
        {
            await _clientService.CreateClientAsync(model.ClientName);
            clientResult = await _clientService.GetClientByNameAsync(model.ClientName);
        }

        projectForm.ClientId = clientResult.Result!.Id;

        await _projectService.UpdateProjectAsync(projectForm);

        return RedirectToAction("Projects");
    }

    [HttpPost]
    [Route("delete")]
    public async Task<IActionResult> DeleteProject(string id)
    {
        await _projectService.DeleteProjectAsync(id);

        return RedirectToAction("Projects");
    }
}
