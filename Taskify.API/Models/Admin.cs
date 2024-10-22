using System.ComponentModel.DataAnnotations;

namespace Taskify.API.Models
{
    public class Admin : BaseModel
    {
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;
    }
}
