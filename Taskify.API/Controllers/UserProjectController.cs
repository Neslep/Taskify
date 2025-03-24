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

[Route("api/user-projects")]
public class UserProjectController : BaseController<UserProjectController>
{
    private readonly IUserProjectRepository _userProjectRepository;

    public UserProjectController(IUserProjectRepository userProjectRepository)
    {
        _userProjectRepository =
            userProjectRepository ?? throw new ArgumentNullException(nameof(userProjectRepository));
    }

    [HttpGet]
    [Route("{projectId}/members")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> GetAllMembersByProjectId(int projectId)
    {
        var members = await _userProjectRepository.GetAllMembersByProjectIdAsync(projectId);
        if (members == null || !members.Any())
        {
            return NotFound("No members found for this project.");
        }

        var response = LazyMapper.Mapper.Map<UserProjectResponse[]>(members);
        return CreateResponse(true, "Request processed successfully.", HttpStatusCode.OK, response);
    }

    [HttpPost]
    [Route("{projectId}/members")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> AddMembersToProject(int projectId, AddMemberProjectRequest request)
    {
        var currentUserId = GetUserIdFromToken();
        // Verify that the current user is the owner
        if (!currentUserId.HasValue)
        {
            return Unauthorized("User ID not found in token.");
        }
        var currentUserProject = await _userProjectRepository.GetByUserIdAsync(currentUserId.Value, projectId);
        if (currentUserProject == null || currentUserProject.RoleInProject != ProjectRole.Owner)
        {
            return Unauthorized("Only the project owner can add members.");
        }

        if (request.MemberEmails == null || !request.MemberEmails.Any())
        {
            return BadRequest("Member emails cannot be null or empty.");
        }

        // Map the request to the entity using LazyMapper.
        var userProjectEntity = LazyMapper.Mapper.Map<UserProject>(request);
        userProjectEntity.ProjectId = projectId;

        var userIds = await _userProjectRepository.GetUserIdsByEmailsAsync(request.MemberEmails);
        await _userProjectRepository.AddAsync(userProjectEntity, userIds);

        return CreateResponse(true, "Members added successfully.", HttpStatusCode.OK, request);
    }

    [HttpDelete]
    [Route("{projectId}/members/{email}")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> RemoveMemberFromProjectByEmail(int projectId, string email)
    {
        var currentUserId = GetUserIdFromToken();
        // Verify that the current user is the owner
        
        if (!currentUserId.HasValue)
        {
            return Unauthorized("User ID not found in token.");
        }
        var currentUserProject = await _userProjectRepository.GetByUserIdAsync(currentUserId.Value, projectId);
        if (currentUserProject == null || currentUserProject.RoleInProject != ProjectRole.Owner)
        {
            return Unauthorized("Only the project owner can remove members.");
        }

        bool isRemoved = await _userProjectRepository.RemoveMemberFromProjectByEmailAsync(email, projectId);
        if (!isRemoved)
        {
            return NotFound($"No member with email {email} found in Project {projectId}.");
        }

        return CreateResponse(true, "Request processed successfully.", HttpStatusCode.OK,
            "Removed member " + email + " successfully");
    }
}

