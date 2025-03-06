using Taskify.API.Enums;

namespace Taskify.API.DTOs.Responses.UserDTOs
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public Gender Gender { get; set; }
        public UserStatus Status { get; set; }
        public PlanType Plans { get; set; }
        public string AvatarPath { get; set; } = string.Empty;
    }
}
