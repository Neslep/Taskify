using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Taskify.API.DTOs.Requests;
using Taskify.API.DTOs.Responses.ProjectDTOs;
using Taskify.API.Enums;
using Taskify.API.Mapper;
using Taskify.API.Models;
using Taskify.API.Services.Repositories.IRepositories;

namespace Taskify.API.Controllers;

[Route("api/projects")]
public class ProjectController : BaseController<ProjectController>
{
    private readonly IProjectRepository _projectRepositories;
    private readonly IUserRepository _userRepository;
    private readonly IUserProjectRepository _userProjectRepository;

    public ProjectController(IProjectRepository projectRepository, IUserRepository userRepository, IUserProjectRepository userProjectRepository)
    {
        _projectRepositories = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
        _userRepository = userRepository;
        _userProjectRepository = userProjectRepository;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> CreateProject(CreateProjectRequest request)
    {
        var userId = GetUserIdFromToken();

        // Map the project from the request
        var project = LazyMapper.Mapper.Map<Project>(request);
        project.OwnerId = userId.GetValueOrDefault();

        // Ensure UserId is set before adding UserProject
        if (!project.UserProjects.Any(up =>
                up.UserId == userId.GetValueOrDefault() && up.RoleInProject == ProjectRole.Owner))
        {
            project.UserProjects.Add(new UserProject
            {
                UserId = userId.GetValueOrDefault(),
                RoleInProject = ProjectRole.Owner
            });
        }

        if (request.MemberEmails != null)
        {
            var validEmails = request.MemberEmails.Where(email => !string.IsNullOrWhiteSpace(email)).ToList();
            foreach (var email in validEmails)
            {
                var user = await _userRepository.GetUserByEmailAsync(email);
                if (user == null)
                {
                    throw new Exception($"User with email {email} not found.");
                }

                project.UserProjects.Add(new UserProject
                {
                    UserId = user.Id,
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
        var projects = await _projectRepositories.GetAllAsync(x =>
            userId != null && (x.OwnerId == userId.Value || x.UserProjects.Any(up => up.UserId == userId.Value)));

        var response = LazyMapper.Mapper.Map<IEnumerable<ProjectResponse>>(projects);

        return CreateResponse(true, "Request processed successfully.", HttpStatusCode.OK, response);
    }

    [HttpPut]
    [Route("{id}")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> UpdateProject(UpdateProjectRequest request, int id)
    {
        var userId = GetUserIdFromToken();
        var project = await _projectRepositories.GetByIdAsync(id);

        if (project.OwnerId != userId)
        {
            throw new Exception("You are not authorized to update this project.");
        }


        var updatedProject = LazyMapper.Mapper.Map(request, project);

        if (request.MemberEmails != null)
        {
            var validEmails = request.MemberEmails.Where(email => !string.IsNullOrWhiteSpace(email)).ToList();
            foreach (var email in validEmails)
            {
                var user = await _userRepository.GetUserByEmailAsync(email);
                if (user == null)
                {
                    return BadRequest($"User with email {email} not found.");
                }

                var isEmailInProject = await _userProjectRepository.IsEmailInProjectAsync(email, id);
                if (isEmailInProject)
                {
                    var existingUserProject = updatedProject.UserProjects.FirstOrDefault(up => up.UserId == user.Id);
                    if (existingUserProject != null)
                    {
                        updatedProject.UserProjects.Remove(existingUserProject);
                    }
                }
                else
                {
                    updatedProject.UserProjects.Add(new UserProject
                    {
                        UserId = user.Id,
                        RoleInProject = ProjectRole.Member
                    });
                }
            }
        }
        
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
        if (project.OwnerId != userId)
        {
            throw new Exception("You are not authorized to update this project.");
        }

        await _projectRepositories.DeleteAsync(project);
        return CreateResponse(true, "Request processed successfully.", HttpStatusCode.OK,
            "Delete project " + project.Id + " successfully");
    }
}