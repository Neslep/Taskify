using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Taskify.API.DTOs.Requests;
using Taskify.API.Enums;
using Taskify.API.Exceptions;
using Taskify.API.Mapper;
using Taskify.API.Models;
using Taskify.API.Services.Repositories.IRepositories;

namespace Taskify.API.Controllers;

[Route("api/projects")]
public class ProjectController : BaseController<ProjectController>
{
    private readonly IProjectRepository _projectRepositories;
    private readonly IUserRepository _userRepository;

    public ProjectController(IProjectRepository projectRepository)
    {
        _projectRepositories = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
    }

    [HttpPost]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> CreateProject(CreateProjectRequest request)
    {
        var userId = GetUserIdFromToken();
        

        // Map the project from the request
        var project = LazyMapper.Mapper.Map<Project>(request);
        project.OwnerId = userId.GetValueOrDefault();
        
        project.UserProjects.Add(new UserProject
        {
            UserId = userId.GetValueOrDefault(),
            RoleInProject = ProjectRole.Owner
        });
        
        if (request.MemberIds != null)
        {
            foreach (var memberId in request.MemberIds)
            {
                project.UserProjects.Add(new UserProject
                {
                    UserId = memberId,
                    RoleInProject = ProjectRole.Member
                });
            }
        }


        project = await _projectRepositories.AddAsync(project);
        return CreateResponse(true, "Request processed successfully.", HttpStatusCode.OK,
            "Add project " + project.Id + " successfully");
    }

    [HttpGet]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> GetProjects()
    {
        var userId = GetUserIdFromToken();
        var projects = await _projectRepositories.GetAllAsync(x => x.OwnerId == userId.Value || x.UserProjects.Any(up => up.UserId == userId.Value));
        
        
        return CreateResponse(true, "Request processed successfully.", HttpStatusCode.OK, projects);
    }

    [HttpPut]
    [Route("{id}")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> UpdateProject(UpdateProjectRequest request, int id)
    {
        var userId = GetUserIdFromToken();
        var project = await _projectRepositories.GetByIdAsync(id);
        if (project == null) throw new NotFoundException("Project not found.");
        
        var userProject = project.UserProjects.FirstOrDefault(x => x.UserId == userId);
        if (userProject == null || userProject.RoleInProject != ProjectRole.Owner)
        {
            throw new Exception("Forbidden: Only the owner can update the project.");
        }
        
        var updatedProject = LazyMapper.Mapper.Map(request, project);
        await _projectRepositories.UpdateAsync(updatedProject);
        return CreateResponse(true, "Request processed successfully.", HttpStatusCode.OK,
            "Update project " + updatedProject.Id + " successfully");
    }

    [HttpDelete]
    [Route("{id}")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        var userId = GetUserIdFromToken();
        var project = await _projectRepositories.GetByIdAsync(id);
        if (project == null) throw new NotFoundException("Project not found.");
        
        var userProject = project.UserProjects.FirstOrDefault(x => x.UserId == userId);
        if (userProject == null || userProject.RoleInProject != ProjectRole.Owner)
        {
            throw new Exception("Forbidden: Only the owner can delete the project.");
        }

        await _projectRepositories.DeleteAsync(project);
        return CreateResponse(true, "Request processed successfully.", HttpStatusCode.OK,
            "Delete project " + project.Id + " successfully");
    }
}