using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Taskify.API.Models
{
    public class Calendar : BaseModel
    {
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
        [Required]
        [MaxLength(255)]
        public string EventName { get; set; } = string.Empty;
        [Required]
        public DateTime EventDate { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string Description { get; set; } = string.Empty;
    }
}
