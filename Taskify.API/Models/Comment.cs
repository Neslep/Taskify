using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Taskify.API.Models
{
    public class Comment : BaseModel
    {
        public int TaskId { get; set; }
        [ForeignKey(nameof(TaskId))]
        public Task? Task { get; set; }
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string Content { get; set; } = string.Empty;
    }
}
