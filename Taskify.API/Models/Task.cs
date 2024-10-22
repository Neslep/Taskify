using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Taskify.API.Enums;

namespace Taskify.API.Models
{
    public class Task : BaseModel
    {
        [Required]
        [MaxLength(255)]
        public string TaskName { get; set; } = string.Empty;
        [Column(TypeName = "nvarchar(max)")]
        public string Description { get; set; } = string.Empty;
        public int? ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public Project? Project { get; set; }
        public int? AssignedUserId { get; set; }
        [ForeignKey(nameof(AssignedUserId))]
        public User? AssignedUser { get; set; }
        [Required]
        public Enums.TaskStatus Status { get; set; }
        [Required]
        public PriorityLevel Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public ICollection<Kanban> Kanbans { get; set; } = new List<Kanban>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
