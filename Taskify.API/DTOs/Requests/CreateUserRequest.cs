using Taskify.API.Enums;

namespace Taskify.API.DTOs.Requests
{
    public record CreateUserRequest(
        string UserName,
        string Email,
        string PasswordHash,
        string PhoneNumber,
        string Address,
        Gender Gender,
        UserStatus Status,
        PlanType Plans,
        string AvatarPath
    );
}
