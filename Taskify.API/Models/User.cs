using System.ComponentModel.DataAnnotations;
using Taskify.API.Enums;

namespace Taskify.API.Models
{
    public class User : BaseModel
    {
        [Required]
        [MaxLength(255)]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;
        [MaxLength(15)]
        public string PhoneNumber { get; set; } = string.Empty;
        [MaxLength(255)]
        public string Address { get; set; } = string.Empty;
        [Required]
        public Gender Gender { get; set; }
        [Required]
        public UserStatus Status { get; set; }
        [Required]
        public PlanType Plans { get; set; }
        [MaxLength(255)]
        public string AvatarPath { get; set; } = string.Empty;
        public ICollection<UserProject> UserProjects { get; set; } = new List<UserProject>();
        public ICollection<Calendar> Calendars { get; set; } = new List<Calendar>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
