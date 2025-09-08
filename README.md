#  HR Management System

![.NET](https://img.shields.io/badge/.NET-8.0-purple)
![C#](https://img.shields.io/badge/C%23-12.0-blue)
![SQLite](https://img.shields.io/badge/SQL%20Server-Database-red)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.0-purple)
![License](https://img.shields.io/badge/License-Portfolio-green)
![South Africa](https://img.shields.io/badge/Market-South%20Africa-green)

A comprehensive Human Resources Management System built with **ASP.NET Core 8**, featuring advanced employee management, department organization, role-based security, and enterprise-grade reporting capabilities.

##  **Key Features**

### **Core Employee Management**
- ? **Complete CRUD Operations** - Create, read, update, delete employees with profile pictures
- ? ** Employee Status Tracking** - Active, OnLeave, Inactive status management
- ? ** Emergency Contact System** - Safety compliance with contact information
- ? ** Birthday Notifications** - Automatic birthday tracking and notifications
- ? **Advanced Search & Filtering** - Find employees by name, email, department, salary range, status
- ? ** Bulk Operations** - Select multiple employees for export or status updates
- ? **Data Pagination** - Efficient handling of large datasets (5 records per page)

### **Department Organization**
- ? **Department Management** - Create and manage company departments
- ? **Employee Assignment** - Link employees to departments with relationship tracking
- ? **Department Analytics** - Employee counts and distribution statistics

### ** Enhanced Dashboard & Analytics**
- ? **Visual Dashboard** - Charts and statistics with Chart.js integration
- ? ** Recent Hires Widget** - Track new employees hired in last 30 days
- ? ** Birthday Notifications Panel** - Upcoming birthdays in next 30 days
- ? ** Employee Status Distribution** - Visual breakdown of Active/OnLeave/Inactive
- ? **Department Distribution Charts** - Visual representation of team structure
- ? **Gender Analytics** - Diversity reporting and analytics
- ? **Salary Analytics** - Average salary calculations in ZAR

### **Advanced Features**
- ? **REST API Endpoints** - Complete JSON API for external integration
- ? **Enterprise Audit Trail** - Track all system changes with user attribution
- ? **Data Export Capabilities** - Excel and PDF export (all employees or selected)
- ? ** Bulk Export** - Export selected employees to Excel with professional formatting
- ? **File Upload System** - Profile picture management with validation
- ? **South African Localization** - ZAR currency formatting and local conventions

### **Security & Compliance**
- ? **Role-Based Access Control** - Admin and HR user permissions
- ? **Complete Audit Trail** - Track who changed what and when
- ? **Secure Authentication** - ASP.NET Core Identity integration
- ? **Input Validation** - Comprehensive data validation and sanitization
- ? **CSRF Protection** - Security against cross-site request forgery

##  **Technology Stack**

| Technology | Purpose | Version |
|------------|---------|---------|
| **ASP.NET Core** | Web framework | 8.0 |
| **Entity Framework Core** | Database ORM | 8.0 |
| **SQLite Database** | Database engine | Latest |
| **ASP.NET Core Identity** | Authentication & Authorization | 8.0 |
| **Bootstrap** | Responsive UI framework | 5.0 |
| **Chart.js** | Data visualization | Latest |
| **ClosedXML** | Excel export | Latest |
| **QuestPDF** | PDF generation | Latest |
| **Font Awesome** | Icons and styling | 6.0 |

##  **Live Demo**

** Live Application:** https://hrmanagement20250823232308-hfaf9f5g2c5fcfe.canadacentral-01.azurewebsites.net  
** Mobile Responsive:** Works on all devices  
** Demo Login:** admin@hrsystem.com / Admin@123  

**API Endpoints:**
- **Health Check:** `GET /api/health`
- **Employees:** `GET /api/EmployeesApi`
- **Departments:** `GET /api/DepartmentsApi`
- **Search:** `GET /api/EmployeesApi/search?term=sarah`

##  **Getting Started**

### **Prerequisites**
- .NET 8 SDK
- SQLite Database (included with Visual Studio)
- Modern web browser

### **Installation**

1. **Clone the repository**
   ```bash
   git clone https://github.com/AyaSox/hr-management-system.git
   cd hr-management-system
   ```

2. **Install dependencies**
   ```bash
   dotnet restore
   ```

3. **Update database**
   ```bash
   dotnet ef database update
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

5. **Open browser**
   ```
   Navigate to: https://localhost:5193
   ```

##  **Default Login Credentials**

| Role | Email | Password | Permissions |
|------|-------|----------|-------------|
| **Admin** | admin@hrsystem.com | Admin@123 | Full system access, delete operations, audit trail |
| **HR** | Create additional users | Custom | Employee management, no delete permissions |

##  **API Documentation**

### **Employee Management API**
| Method | Endpoint | Purpose | Auth Required |
|--------|----------|---------|---------------|
| GET | `/api/EmployeesApi` | Get all employees | ? |
| GET | `/api/EmployeesApi/{id}` | Get specific employee | ? |
| POST | `/api/EmployeesApi` | Create new employee | ? |
| PUT | `/api/EmployeesApi/{id}` | Update employee | ? |
| DELETE | `/api/EmployeesApi/{id}` | Delete employee | Admin Only |
| GET | `/api/EmployeesApi/search?term={searchTerm}` | Search employees | ? |

### **Department Management API**
| Method | Endpoint | Purpose | Auth Required |
|--------|----------|---------|---------------|
| GET | `/api/DepartmentsApi` | Get all departments with counts | ? |
| GET | `/api/DepartmentsApi/{id}` | Get department with employees | ? |
| POST | `/api/DepartmentsApi` | Create new department | ? |
| GET | `/api/DepartmentsApi/{id}/employees` | Get department employees | ? |

### **Health Monitoring API**
| Method | Endpoint | Purpose | Auth Required |
|--------|----------|---------|---------------|
| GET | `/api/health` | Basic health status | ? |
| GET | `/api/health/ready` | Application readiness | ? |

##  **New Features Highlights**

### ** Employee Status Management**
- Track employee status: Active, OnLeave, Inactive
- Visual status badges in employee list
- Filter and search by status
- Bulk status updates with audit trail

### ** Advanced Search & Filtering**
- **Salary Range Search:** Filter by minimum and maximum salary
- **Multi-Criteria Filtering:** Combine name, department, salary, and status filters
- **Enhanced Sorting:** Sort by name, salary, and hire date
- **Persistent Filters:** Maintain filter state across pagination

### ** Birthday Notification System**
- **Upcoming Birthdays Widget:** Dashboard panel showing next 30 days
- **Birthday Indicators:** Cake icons in employee list for upcoming birthdays
- **Age Calculations:** Automatic age computation
- **Smart Date Logic:** Handles year transitions and leap years

### ** Recent Hires Tracking**
- **Recent Hires Widget:** Dashboard showing employees hired in last 30 days
- **Hiring Analytics:** Track recruitment patterns
- **Quick Overview:** New team member information at a glance

### ** Emergency Contact Management**
- **Safety Compliance:** Emergency contact name and phone number
- **Business Requirement:** Critical information for HR departments
- **Professional Standards:** Meets workplace safety requirements

### ** Bulk Operations**
- **Multi-Select Interface:** Checkbox selection for multiple employees
- **Bulk Export:** Export selected employees to Excel
- **Bulk Status Updates:** Change status for multiple employees simultaneously
- **Audit Integration:** All bulk operations logged with user attribution

##  **Business Value**

### **For HR Departments**
- **Complete Employee Lifecycle:** From hire to termination tracking
- **Compliance Ready:** Emergency contacts, audit trails, role-based access
- **Efficiency Tools:** Bulk operations, advanced search, data export
- **Analytics & Reporting:** Visual dashboards, birthday tracking, hire analytics

### **For Management**
- **Business Intelligence:** Department distribution, salary analytics, status reports
- **Decision Support:** Visual charts, trend analysis, employee statistics
- **Operational Efficiency:** Quick search, bulk operations, automated notifications

### **For IT Departments**
- **Integration Ready:** REST APIs for connecting external systems
- **Security Compliant:** Role-based access, audit trails, input validation
- **Scalable Architecture:** Clean code, proper separation of concerns
- **Modern Technology:** Latest .NET 8, cloud-ready deployment

##  **South African Market Focus**

- **Currency Support:** ZAR formatting throughout the system
- **Local Conventions:** South African employee names and contact formats
- **Professional Standards:** Corporate governance and compliance considerations
- **Market Appropriate:** Salary ranges and business practices for SA market

##  **Performance & Scalability**

- **Optimized Queries:** Efficient LINQ with proper indexing
- **Pagination:** Handle large datasets with 5 records per page
- **Search Performance:** Database-level filtering and sorting
- **Memory Management:** Efficient resource utilization
- **Connection Pooling:** Optimized database connections

##  **Security Features**

- **Authentication:** ASP.NET Core Identity with secure password storage
- **Authorization:** Role-based permissions with method-level security
- **Input Validation:** Comprehensive validation on all forms
- **CSRF Protection:** Protection against cross-site request forgery
- **File Upload Security:** Validated file types and size limits
- **Audit Trail:** Complete change tracking for compliance

##  **Deployment**

### **Local Development**
- SQLite Database for development
- Development environment with detailed logging
- Hot reload for rapid development

### **Azure Production**
- Microsoft Azure App Service deployment
- Production-optimized configuration
- In-memory database for reliable demo
- Professional demo data seeding

##  **Screenshots**

### **Enhanced Dashboard**
- Recent hires widget with last 30 days
- Birthday notifications panel
- Employee status distribution chart
- Interactive analytics with Chart.js

### **Advanced Employee Management**
- Multi-criteria search and filtering
- Bulk selection and operations
- Status badges and birthday indicators
- Professional profile management

### **Modern User Interface**
- Bootstrap 5 responsive design
- Professional business styling
- Interactive bulk operations
- Mobile-optimized interface

---

## ? **Developer Portfolio**

**Developed by:** AyaSox  
**GitHub:** https://github.com/AyaSox/hr-management-system  
**Live Demo:** https://hrmanagement20250823232308-hfaf9f5g2c5fcfe.canadacentral-01.azurewebsites.net  

### **Skills Demonstrated**
- **Full-Stack Development:** ASP.NET Core 8, Entity Framework, SQLite
- **Modern UI/UX:** Bootstrap 5, Chart.js, responsive design
- **Cloud Deployment:** Microsoft Azure App Service
- **API Development:** RESTful JSON endpoints
- **Security Implementation:** Authentication, authorization, audit trails
- **Business Logic:** Real-world HR management processes
- **Database Design:** Proper relationships, migrations, optimization

---

##  **License**

This project is available for portfolio demonstration purposes.

---

