using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Taskify.API.Enums;

namespace Taskify.API.Models
{
    public class Kanban : BaseModel
    {
        public int ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public Project? Project { get; set; }
        public int TaskId { get; set; }
        [ForeignKey(nameof(TaskId))]
        public Task? Task { get; set; }
        [Required]
        public KanbanStatus Status { get; set; }
    }
}
