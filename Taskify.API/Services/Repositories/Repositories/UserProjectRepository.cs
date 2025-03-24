using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Taskify.API.Data;
using Taskify.API.Enums;
using Taskify.API.Exceptions;
using Taskify.API.Models;
using Taskify.API.Services.Repositories.IRepositories;
using Task = System.Threading.Tasks.Task;

namespace Taskify.API.Services.Repositories.Repositories;

public class UserProjectRepository : RepositoryBase, IUserProjectRepository
{

    public UserProjectRepository(TaskifyContext dbContext) : base(dbContext)
    {
    }
    
    public async Task<UserProject?> GetByUserIdAsync(int userId, int projectId)
    {
        return await _dbContext.UserProjects.FindAsync(userId, projectId);
    }


    public Task<UserProject?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<UserProject> AddAsync(UserProject entity)
    {
        entity.LastModifiedDate = DateTime.UtcNow;

        await _dbContext.UserProjects.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }
    
    public async Task<UserProject> AddAsync(UserProject entity, IEnumerable<int> userIds)
    {
        
        // Create user project entries for each member.
        var userProjects = userIds.Select(userId => new UserProject
        {
            UserId = userId,
            ProjectId = entity.ProjectId,
            RoleInProject = ProjectRole.Member,
            LastModifiedDate = DateTime.UtcNow
        });
    
        await _dbContext.UserProjects.AddRangeAsync(userProjects);
        await _dbContext.SaveChangesAsync();

        return entity;
    }
    
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
    
   
  

    public async Task<bool> DeleteAsync(UserProject entity)
    {
        _dbContext.UserProjects.Remove(entity);
        return await _dbContext.SaveChangesAsync() > 0;
    }
    
    public async Task<bool> RemoveMemberFromProjectByEmailAsync(string email, int projectId)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            throw new NotFoundException($"User with email {email} not found.");
        }

        var userProject = await _dbContext.UserProjects.FindAsync(user.Id, projectId);
        if (userProject == null)
        {
            throw new NotFoundException($"Member with email {email} is not part of Project {projectId}.");
        }

        _dbContext.UserProjects.Remove(userProject);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAsync(UserProject entity)
    {
        var userProject = await _dbContext.UserProjects.FindAsync(entity.UserId, entity.ProjectId);
        if (userProject == null) throw new NotFoundException($"UserProject with UserId {entity.UserId} and ProjectId {entity.ProjectId} not found.");

        // Update properties here if needed
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<IReadOnlyList<UserProject>> GetAllAsync(Expression<Func<UserProject, bool>> predicate)
    {
        var userProjects = await _dbContext.UserProjects
            .Include(up => up.User)
            .Include(up => up.Project)
            .Where(predicate)
            .ToListAsync();

        if (!userProjects.Any())
        {
            throw new NotFoundException("No userProjects found.");
        }

        return userProjects;
    }

    public async Task<bool> IsEmailInProjectAsync(string email, int projectId)
    {
        var userProject = await _dbContext.UserProjects
            .Include(up => up.User)
            .FirstOrDefaultAsync(up => up.User.Email == email && up.ProjectId == projectId);

        return userProject != null;
    }
    
    public async Task<IEnumerable<UserProject>> GetAllMembersByProjectIdAsync(int projectId)
    {
        return await _dbContext.UserProjects
            .Where(up => up.ProjectId == projectId)
            .Include(up => up.User)
            .ToListAsync() ?? new List<UserProject>(); // Đảm bảo không trả về null
    }

    public async Task<IEnumerable<int>> GetUserIdsByEmailsAsync(IEnumerable<string> emails)
    {
        return await _dbContext.Users
            .Where(u => emails.Contains(u.Email))
            .Select(u => u.Id)
            .ToListAsync();
    }
    

   
}