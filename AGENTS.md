# AGENTS.md - LeaveManagementSystem
## Purpose
This repository is an ASP.ET Core MVC Leave Management System using EF Core, SQL Server,
 and Identity for authentication and authorization. It provides a web-based interface for employees to request leave, managers to approve or reject leave requests, and administrators to manage users and leave policies.

 Agents must preserve the layered architecture of the application, ensuring that the presentation layer, business logic layer, and data access layer remain separate. This separation of concerns allows for easier maintenance, testing, and scalability of the application and modernize toward .NET 10.

 ## Setup commands

 use these commands from repository root:

 ' ' 'bash
 dotnet --info
 dotnet restore LeaveManagementSystem.sln
 dotnet build LeaveManagementSystem.sln --configuration Release --no-restore
 dotnet test LeaveManagementSystem.sln --configuration Release --no-build