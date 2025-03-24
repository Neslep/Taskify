namespace Taskify.API.DTOs.Requests;

public record AddMemberProjectRequest(
    List<string>? MemberEmails
    );