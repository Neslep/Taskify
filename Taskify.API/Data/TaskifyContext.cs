using Microsoft.EntityFrameworkCore;
using Taskify.API.Models;

namespace Taskify.API.Data
{
    public class TaskifyContext : DbContext
    {
        public TaskifyContext()
        {
        }

        public TaskifyContext(DbContextOptions<TaskifyContext> options) : base(options)
        {
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<UserProject> UserProjects { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<Kanban> Kanbans { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Todolist> Todolists { get; set; }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseModel>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.UtcNow;
                        break;
                }
            }
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships for UserProject (many-to-many between Users and Projects)
            modelBuilder.Entity<UserProject>()
                .HasKey(up => new { up.UserId, up.ProjectId });

            modelBuilder.Entity<UserProject>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserProjects)
                .HasForeignKey(up => up.UserId);

            modelBuilder.Entity<UserProject>()
                .HasOne(up => up.Project)
                .WithMany(p => p.UserProjects)
                .HasForeignKey(up => up.ProjectId);

            // Configure relationships for Task and Project
            modelBuilder.Entity<Models.Task>()
                .HasOne(t => t.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.ProjectId);

            // Configure relationships for Calendar and User
            modelBuilder.Entity<Calendar>()
                .HasOne(c => c.User)
                .WithMany(u => u.Calendars)
                .HasForeignKey(c => c.UserId);

            // Configure relationships for Kanban and Project, Task
            modelBuilder.Entity<Kanban>()
                .HasOne(k => k.Project)
                .WithMany(p => p.Kanbans)
                .HasForeignKey(k => k.ProjectId);

            modelBuilder.Entity<Kanban>()
                .HasOne(k => k.Task)
                .WithMany(t => t.Kanbans)
                .HasForeignKey(k => k.TaskId);

            // Configure relationships for Comment and Task, User
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Task)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.TaskId);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId);

            // Configure relationships for Todolist and Project
            modelBuilder.Entity<Todolist>()
                .HasOne(tl => tl.Project)
                .WithMany(p => p.Todolists)
                .HasForeignKey(tl => tl.ProjectId);
        }
    }
}
