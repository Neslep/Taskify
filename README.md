# Taskify

Taskify is a task and project management application designed to help teams and individuals manage their tasks efficiently. With features like project collaboration, task tracking, Kanban board, personal calendars, and more, Taskify is perfect for office workers, students, freelancers, and teachers who need a comprehensive solution to stay organized.

## Features
- User Management: Sign up, login, and manage user profiles.
- Project Management: Create, manage, and assign projects with specific roles like Owner and Member.
- Task Management: Manage tasks within projects, assign tasks to users, and track the status of tasks.
- Kanban Board: Visualize project progress through a Kanban board.
- Todo List: A shared to-do list for project members.
- Personal Calendar: Users can manage their own events and appointments through the personal calendar.
- Comments: Users can leave comments on tasks to enhance collaboration.
- Role Management: Differentiate between Admin, SuperAdmin, and regular users.

## Table of Contents
- Features
- Installation
- Technologies
- Database Schema
- How to Contribute
- License

## Installation
To get started with Taskify, follow the steps below:

### Prerequisites
- .NET 8 or higher installed on your machine
- SQL Server or any other compatible database
- A package manager like NuGet

### Clone the Repository
```bash
git clone https://github.com/neslep/taskify.git
cd taskify
```

### Configure the Database
1. In the appsettings.json, add your database connection string under ConnectionStrings:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your_server;Database=TaskifyDb;User Id=your_user;Password=your_password;TrustServerCertificate=True;"
  }
}
```
2. Run the following commands to apply migrations and update the database:
```bash
Add-Migration InitialCreate
Update-Database
```

### Build and Run the Project
```bash
dotnet build
dotnet run
```
This will launch the application at http://localhost:7097.

## Technologies
- ASP.NET Core 8: Backend framework
- Entity Framework Core: ORM for database access
- SQL Server: Database
- Razor: UI framework for building the frontend
- C#: Programming language
- JWT: For authentication and authorization
- Docker (optional): Containerization for easier deployment

## Database Schema
The project contains the following key tables:
- Users: Stores information about users including their roles and details.
- Projects: Stores project-related data including ownership.
- Tasks: Stores tasks and their statuses.
- Kanbans: Represents task progress on a Kanban board.
- Todolists: Shared task list for project members.
- Comments: Users can leave comments on tasks.

## How to Contribute
If you'd like to contribute to this project, feel free to open a pull request. Follow these steps:
1. Fork the repository.
2. Create a new branch (git checkout -b feature/your-feature).
3. Commit your changes (git commit -m 'Add some feature').
4. Push to the branch (git push origin feature/your-feature).
5. Open a Pull Request.

## License
This project is licensed under the MIT License - see the LICENSE file for details.
