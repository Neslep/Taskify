using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Taskify.API.Data;
using Taskify.API.Exceptions;
using Taskify.API.Models;
using Taskify.API.Services.Repositories.IRepositories;

namespace Taskify.API.Services.Repositories.Repositories;

public class UserProjectRepository : RepositoryBase, IUserProjectRepository
{
    
    public UserProjectRepository(TaskifyContext dbContext) : base(dbContext)
    {
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
    
    public async Task<bool> DeleteAsync(UserProject entity)
    {
        _dbContext.UserProjects.Remove(entity);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAsync(UserProject entity)
    {
        var userProject = await _dbContext.UserProjects.FindAsync(entity.Id);
        if (userProject == null) throw new NotFoundException($"UserProject with ID {entity.Id} not found.");
        
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
    

}