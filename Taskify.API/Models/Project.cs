using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Taskify.API.Enums;

namespace Taskify.API.Models
{
    public class Project : BaseModel
    {
        [Required]
        [MaxLength(255)]
        public string ProjectName { get; set; } = string.Empty;
        [Column(TypeName = "nvarchar(max)")]
        public string Description { get; set; } = string.Empty;
        public ProjectStatus ProjectStatus { get; set; }
        public int? OwnerId { get; set; }
        [ForeignKey(nameof(OwnerId))]
        public User? Owner { get; set; }
        public ICollection<UserProject> UserProjects { get; set; } = new List<UserProject>();
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
        public ICollection<Kanban> Kanbans { get; set; } = new List<Kanban>();
        public ICollection<Todolist> Todolists { get; set; } = new List<Todolist>();
    }
}
