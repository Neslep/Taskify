using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Taskify.API.Data;
using Taskify.API.Exceptions;
using Taskify.API.Models;
using Taskify.API.Services.Repositories.IRepositories;

namespace Taskify.API.Services.Repositories.Repositories;

public class ProjectRepository : RepositoryBase, IProjectRepository
{
    public ProjectRepository(TaskifyContext dbContext) : base(dbContext)
    {
    }

    public async Task<Project> AddAsync(Project entity)
    {
        entity.LastModifiedDate = DateTime.UtcNow;
        
        await _dbContext.Projects.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }
   
    public async Task<bool> DeleteAsync(Project entity)
    {
        _dbContext.Projects.Remove(entity);
        return await _dbContext.SaveChangesAsync() > 0;
    }
    
    public async Task<bool> UpdateAsync(Project entity)
    {
        var project = await _dbContext.Projects.FindAsync(entity.Id);
        if (project == null) throw new NotFoundException($"Project with ID {entity.Id} not found.");
        
        project.ProjectName = entity.ProjectName;
        project.Description = entity.Description;
        project.ProjectStatus = entity.ProjectStatus;
        project.LastModifiedDate = DateTime.UtcNow;
        
        _dbContext.Projects.Update(project);
        return await _dbContext.SaveChangesAsync() > 0;
    }
    
    public async Task<IReadOnlyList<Project>> GetAllAsync(Expression<Func<Project, bool>> predicate)
    {
        var projects = await _dbContext.Projects
            .Include(p => p.UserProjects)
            .ThenInclude(up => up.User)
            .Include(p => p.Tasks)
            .Include(p => p.Kanbans)
            .Include(p => p.Todolists)
            .Where(predicate)
            .ToListAsync();

        if (!projects.Any())
        {
            throw new NotFoundException("No projects found.");
        }

        return projects;
        return await _dbContext.Projects.Where(predicate).ToListAsync();
    }

    public async Task<Project?> GetByIdAsync(int id)
    {
        return await _dbContext.Projects.FindAsync(id);
    }
    
    
}