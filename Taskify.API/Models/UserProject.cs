using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Taskify.API.Enums;

namespace Taskify.API.Models
{
    public class UserProject : BaseModel
    {
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
        public int ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public Project? Project { get; set; }
        [Required]
        public ProjectRole RoleInProject { get; set; }
    }
}
