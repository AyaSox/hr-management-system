HR Management System

![.NET](https://img.shields.io/badge/.NET-8.0-purple)
![C#](https://img.shields.io/badge/C%23-12.0-blue)
![SQL Server](https://img.shields.io/badge/SQL%20Server-LocalDB-red)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.0-purple)
![License](https://img.shields.io/badge/License-Portfolio-green)
![South Africa](https://img.shields.io/badge/Market-South%20Africa-green)

A comprehensive Human Resources Management System built with **ASP.NET Core 8**, featuring employee management, department organization, role-based security, and advanced reporting capabilities.

Key Features

Core Functionality
Employee Management - Complete CRUD operations with profile pictures
- Department Management - Organize employees by departments
- Advanced Search & Filtering - Find employees by name, email, or department
- Data Pagination - Efficient handling of large datasets
- Role-Based Security - Admin and HR user permissions

Advanced Features
-  **REST API Endpoints** - Full JSON API for integration
-  **Audit Trail System** - Track all system changes with user attribution
-  **Data Export** - Excel and PDF export capabilities
- ? **Dashboard Analytics** - Visual charts and statistics
- ? **Profile Picture Uploads** - Employee photo management
- ? **South African Localization** - ZAR currency formatting

## ?? **Technology Stack**

| Technology | Purpose |
|------------|---------|
| **ASP.NET Core 8** | Web framework |
| **Entity Framework Core** | Database ORM |
| **SQL Server LocalDB** | Database engine |
| **ASP.NET Core Identity** | Authentication & Authorization |
| **Bootstrap 5** | Responsive UI framework |
| **Chart.js** | Data visualization |
| **ClosedXML** | Excel export |
| **QuestPDF** | PDF generation |

## ?? **Getting Started**

### **Prerequisites**
- .NET 8 SDK
- SQL Server LocalDB (included with Visual Studio)
- Modern web browser

### **Installation**

1. **Clone the repository**
   ```bash
   git clone https://github.com/[your-username]/hr-management-system.git
   cd hr-management-system
   ```

2. **Install dependencies**
   ```bash
   dotnet restore
   ```

3. **Update database**
   ```bash
   dotnet ef database update -c AppDbContext
   dotnet ef database update -c ApplicationDbContext
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

5. **Open browser**
   ```
   Navigate to: https://localhost:5193
   ```

## ?? **Default Login Credentials**

| Role | Email | Password |
|------|-------|----------|
| **Admin** | admin@hrsystem.com | Admin@123 |

*Admin users can delete records and access all features*

## ?? **API Endpoints**

### **Employees API**
- `GET /api/EmployeesApi` - Get all employees
- `GET /api/EmployeesApi/{id}` - Get specific employee
- `POST /api/EmployeesApi` - Create new employee
- `PUT /api/EmployeesApi/{id}` - Update employee
- `DELETE /api/EmployeesApi/{id}` - Delete employee (Admin only)
- `GET /api/EmployeesApi/search?term={searchTerm}` - Search employees

### **Departments API**
- `GET /api/DepartmentsApi` - Get all departments
- `GET /api/DepartmentsApi/{id}` - Get department with employees
- `POST /api/DepartmentsApi` - Create new department
- `GET /api/DepartmentsApi/{id}/employees` - Get department employees

*All API endpoints require authentication*

## ?? **Key Business Features**

### **Employee Management**
- Complete employee lifecycle management
- Profile picture uploads and management
- Department assignments
- Salary tracking in South African Rand (ZAR)
- Hire date tracking

### **Security & Compliance**
- Role-based access control (Admin/HR roles)
- Complete audit trail for all changes
- Secure authentication system
- Input validation and sanitization

### **Reporting & Analytics**
- Visual dashboard with employee statistics
- Department distribution charts
- Gender diversity analytics
- Average salary calculations
- Excel and PDF export capabilities

### **Modern Architecture**
- Clean separation of concerns
- RESTful API design
- Responsive mobile-friendly interface
- Professional error handling
- Performance optimizations

## ?? **Performance Features**

- **Optimized database queries** with Entity Framework
- **Pagination** for large datasets
- **Efficient search** with database-level filtering
- **Background processing** for role seeding
- **Connection pooling** and retry policies

## ?? **Security Features**

- **Authentication** via ASP.NET Core Identity
- **Authorization** with role-based permissions
- **CSRF protection** on all forms
- **Input validation** and sanitization
- **Secure file uploads** with type checking

## ?? **User Experience**

- **Responsive design** works on all devices
- **Intuitive navigation** with clear menu structure
- **Professional styling** with Bootstrap 5
- **Interactive charts** for data visualization
- **Fast search and filtering** capabilities

## ?? **Localization**

- **South African market focus** with ZAR currency
- **Date formatting** appropriate for SA
- **Professional terminology** suitable for local businesses

## ?? **Screenshots**

### Dashboard Analytics
![Dashboard](https://via.placeholder.com/800x400/0066cc/ffffff?text=Dashboard+with+Charts+and+Statistics)

### Employee Management
![Employee List](https://via.placeholder.com/800x400/28a745/ffffff?text=Employee+Management+with+Search+and+Filters)

### Professional Interface
![Modern UI](https://via.placeholder.com/800x400/6f42c1/ffffff?text=Modern+Bootstrap+5+Interface)

---

## ????? **Developer**

**[Your Name]**  
*Full-Stack .NET Developer*  
?? [your-email@example.com]  
?? [LinkedIn Profile]  
?? [GitHub Profile]

---

## ?? **License**

This project is available for portfolio demonstration purposes.

---

*Built with ?? for the South African job market*
