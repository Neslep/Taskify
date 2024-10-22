using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Taskify.API.Enums;

namespace Taskify.API.Models
{
    public class Todolist : BaseModel
    {
        public int ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public Project? Project { get; set; }
        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;
        [Column(TypeName = "nvarchar(max)")]
        public string Description { get; set; } = string.Empty;
        [Required]
        public TodolistStatus Status { get; set; }
    }
}
