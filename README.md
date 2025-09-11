# HR Management System

Overview
This is an HR Management System built with .NET 8 and Razor Pages. It uses SQLite for persistent storage, ASP.NET Core Identity for authentication and role management, Hangfire for background jobs, and QuestPDF for PDF generation. The project includes REST API endpoints, localization support, demo data seeding, and an optional Render deployment configuration.

Key features
- Employee management with detailed model fields: employee number, full name, email, job title, employment type, date hired, date of birth, salary, gender, status, emergency contact, line manager, department, profile picture
- Self-referencing line manager relationships and direct reports navigation
- Soft delete support on Employee
- Calculated and helper properties on Employee: Age, HasUpcomingBirthday, FormattedSalary, LengthOfService, HasUpcomingAnniversary, YearsOfService
- Departments management
- Demo data seeder that populates departments and sample employees and sets line manager relationships
- ASP.NET Core Identity for authentication and admin role support
- REST API controller for employees with create, read, update, delete and search endpoints
- Swagger/OpenAPI for API documentation in development
- Hangfire background job scheduling with recurring tasks for reminders and reports
- Localization and culture setup for en-ZA and en-US
- Session and in-memory caching configured
- File upload support for employee profile pictures
- PDF generation via QuestPDF

Project structure highlights
- Models
  - Employee.cs: domain model with data annotations, relationships, helper properties and profile picture handling
  - Department.cs: department entity and relationship
  - ViewModels: EmployeeListItem, EmployeeImportRow and other DTO and view models
- Data
  - AppDbContext: application database context using SQLite
  - DemoDataSeeder: seeds departments and sample employees
  - Migrations folder with model snapshot for current schema
- Controllers and Pages
  - API controllers for employee operations
  - Razor Pages for the user interface, prioritized for this project
- Services
  - Background job tasks and hosted reminder service
  - Audit and other scoped services
- Program.cs
  - Service registration for DbContexts, Identity, Hangfire, localization, Swagger, QuestPDF, session, memory cache
  - Application started seeding and recurring jobs scheduling
  - Hangfire dashboard registration with authorization filter

Local development and run instructions
1. Prerequisites
   - .NET 8 SDK installed
   - Visual Studio 2022 or other IDE that supports .NET 8
2. Clone the repository and open the solution in the IDE
3. Restore and build
   - dotnet restore
   - dotnet build
4. Configuration
   - Default SQLite file is used when no connection string is present: Data Source=hrmanagement.db
   - Identity database uses a separate SQLite file by replacing the filename in the connection string
5. Run the app locally
   - dotnet run from the project folder
   - Or start from Visual Studio
6. First run behavior
   - Application ensures databases are created and runs the demo data seeder on application start
   - Seeded demo users and roles are created by RoleSeeder and DemoDataSeeder
7. Swagger UI is available in Development environment at /swagger
8. Hangfire dashboard is mounted at /hangfire and can be protected with role-based authorization

Database and migrations
- Uses Entity Framework Core with the SQLite provider
- Migrations folder contains model snapshot
- To add a migration locally:
  - dotnet ef migrations add YourMigrationName --project HRManagementSystem --startup-project HRManagementSystem
  - dotnet ef database update

Authentication and authorization
- ASP.NET Core Identity configured with IdentityUser and IdentityRole using a separate SQLite file
- Identity options are relaxed for demo mode but should be hardened for production
- Hangfire dashboard can be restricted to Admin role in production

API
- EmployeesApiController exposes endpoints under /api/employeesapi for CRUD and search
- API controllers are protected by Authorize. Use configured Identity accounts to make requests or extend with token logic as needed

Background jobs and scheduling
- Hangfire is configured with in-memory storage for demo purposes
- Recurring jobs are scheduled on application startup: birthday reminders, anniversary reminders, monthly and daily reports
- For production use, switch to a persistent Hangfire storage such as SQL Server or Redis and secure the dashboard

Localization and culture
- Localization configured with resources path Resources
- Supported cultures: en-ZA (default) and en-US
- Culture selection is persisted via a cookie endpoint

File uploads
- Employee.ProfilePicture is not mapped to the database, while ProfilePicturePath stores the uploaded file path
- Create and Edit Razor Pages use form enctype multipart/form-data
- Validate uploaded image size and type before saving to disk or external storage
- For deployment, ensure the file storage path is writable or use external storage for persistence across instances

Troubleshooting
- Build errors referencing .NET version: confirm .NET 8 SDK installed and target framework matches
- Database errors: check file path and ensure the process can write to the SQLite file or switch to a server DB for multi-instance deployments
- Hangfire dashboard not accessible: verify authorization configuration and role membership for the signed-in user
- Profile pictures not persisting after redeploy: use external persistent storage instead of instance file system

Contributing
- Fork the repository and submit pull requests
- Follow existing coding conventions and include tests for new features
- Update or add migrations when changing models

Demo credentials for quick access
- admin@hrsystem.com / Admin@123

