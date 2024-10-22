namespace Taskify.API.Enums
{
    public enum Gender
    {
        Male,
        Female,
        Other
    }

    public enum UserStatus
    {
        Active,
        Inactive
    }

    public enum PlanType
    {
        Free,
        Pro
    }

    public enum ProjectRole
    {
        Owner,
        Member
    }

    public enum TaskStatus
    {
        ToDo,
        InProgress,
        Done,
        Cancelled
    }

    public enum PriorityLevel
    {
        Low,
        Medium,
        High
    }

    public enum KanbanStatus
    {
        Backlog,
        InProgress,
        Completed
    }

    public enum TodolistStatus
    {
        Pending,
        Done
    }
}
