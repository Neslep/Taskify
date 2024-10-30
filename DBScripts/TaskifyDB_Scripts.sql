-- Admins Table
CREATE TABLE Admins (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(100) NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    CreatedDate DATETIME2 NOT NULL,
    LastModifiedDate DATETIME2 NOT NULL
);

-- Users Table
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    PhoneNumber NVARCHAR(15),
    Address NVARCHAR(255),
    Gender INT NOT NULL, -- (Male, Female, Other)
    Status INT NOT NULL, -- (Active, Inactive)
    Plans INT NOT NULL, -- (Free, Pro)
    AvatarPath NVARCHAR(255),
    CreatedDate DATETIME2 NOT NULL,
    LastModifiedDate DATETIME2 NOT NULL
);

-- Projects Table
CREATE TABLE Projects (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProjectName NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    OwnerId INT,
    CreatedDate DATETIME2 NOT NULL,
    LastModifiedDate DATETIME2 NOT NULL,
    FOREIGN KEY (OwnerId) REFERENCES Users(Id) ON DELETE CASCADE
);

-- UserProject Tables (N-N link between User and Project)
CREATE TABLE UserProjects (
    UserProjectId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT,
    ProjectId INT,
    RoleInProject INT NOT NULL, -- (Owner, Member)
    CreatedDate DATETIME2 NOT NULL,
    LastModifiedDate DATETIME2 NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (ProjectId) REFERENCES Projects(Id) ON DELETE CASCADE
);

-- Tasks Table
CREATE TABLE Tasks (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TaskName NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    ProjectId INT,
    AssignedUserId INT,
    Status INT NOT NULL, -- (To do, In Progress, Done, Cancelled)
    Priority INT NOT NULL, -- (Low, Medium, High)
    DueDate DATE,
    CreatedDate DATETIME2 NOT NULL,
    LastModifiedDate DATETIME2 NOT NULL,
    FOREIGN KEY (ProjectId) REFERENCES Projects(Id) ON DELETE CASCADE,
    FOREIGN KEY (AssignedUserId) REFERENCES Users(Id)
);

-- Calendars Table (User's personal calendar)
CREATE TABLE Calendars (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT,
    EventName NVARCHAR(255) NOT NULL,
    EventDate DATE NOT NULL,
    Description NVARCHAR(MAX),
    CreatedDate DATETIME2 NOT NULL,
    LastModifiedDate DATETIME2 NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);

-- Kanbans Table (Task status in Project Kanban)
CREATE TABLE Kanbans (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProjectId INT,
    TaskId INT,
    Status INT NOT NULL, -- (Backlog, In Progress, Completed)
    CreatedDate DATETIME2 NOT NULL,
    LastModifiedDate DATETIME2 NOT NULL,
    FOREIGN KEY (ProjectId) REFERENCES Projects(Id) ON DELETE CASCADE,
    FOREIGN KEY (TaskId) REFERENCES Tasks(Id)
);

-- Comments Table (User's Comment on Task)
CREATE TABLE Comments (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TaskId INT,
    UserId INT,
    Content NVARCHAR(MAX) NOT NULL,
    CreatedDate DATETIME2 NOT NULL,
    LastModifiedDate DATETIME2 NOT NULL,
    FOREIGN KEY (TaskId) REFERENCES Tasks(Id) ON DELETE CASCADE,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- Todolists Table (List of tasks in Project)
CREATE TABLE Todolists (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProjectId INT,
    Title NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    Status INT NOT NULL, -- (Pending, Done)
    CreatedDate DATETIME2 NOT NULL,
    LastModifiedDate DATETIME2 NOT NULL,
    FOREIGN KEY (ProjectId) REFERENCES Projects(Id) ON DELETE CASCADE
);
