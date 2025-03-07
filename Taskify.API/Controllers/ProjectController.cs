using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Taskify.API.DTOs.Requests;
using Taskify.API.Exceptions;
using Taskify.API.Mapper;
using Taskify.API.Models;
using Taskify.API.Services.Repositories.IRepositories;

namespace Taskify.API.Controllers;


[Route("api/projects")]
public class ProjectController : BaseController<ProjectController>
{
    private readonly IProjectRepository _projectRepositories;
    
    public ProjectController(IProjectRepository projectRepository)
    {
        _projectRepositories = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> CreateProject(ProjectRequest request)
    {
        Project projects = LazyMapper.Mapper.Map<Project>(request);
        projects = await _projectRepositories.AddAsync(projects);
        return CreateResponse(true, "Request processed successfully.", HttpStatusCode.OK, "Add project " + projects.Id + " successfully");
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> GetProjects()
    {
        var projects = await _projectRepositories.GetAllAsync(x => x.Id > 0);
        return CreateResponse(true, "Request processed successfully.", HttpStatusCode.OK, projects);
    }
    
    [HttpPut]
    [Route("{id}")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> UpdateProject(ProjectRequest request, int id)
    {
        var project = await _projectRepositories.GetByIdAsync(id);
        if (project == null) throw new NotFoundException("Project not found.");
        
        var updatedProject = LazyMapper.Mapper.Map(request, project);
        await _projectRepositories.UpdateAsync(updatedProject);
        return CreateResponse(true, "Request processed successfully.", HttpStatusCode.OK, "Update project " + updatedProject.Id + " successfully");
    }
    
    [HttpDelete]
    [Route("{id}")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        var project = await _projectRepositories.GetByIdAsync(id);
        if (project == null) throw new NotFoundException("Project not found.");
        
        await _projectRepositories.DeleteAsync(project);
        return CreateResponse(true, "Request processed successfully.", HttpStatusCode.OK, "Delete project " + project.Id + " successfully");
    }
}