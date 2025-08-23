# ?? HR Management System - Quick Demo Setup

## For Managers/Reviewers

### ? **Quick Start (1 minute)**

1. **Open Terminal/Command Prompt**
2. **Navigate to project folder**
3. **Run these commands:**
   ```bash
   dotnet run
   ```
4. **Open browser:** `https://localhost:5193`
5. **Login as Admin:**
   - Email: `admin@hrsystem.com`
   - Password: `Admin@123`

### ?? **Key Features to Demonstrate**

#### **Employee Management**
- ? Add new employees with photos
- ? Search and filter employees
- ? Edit employee information
- ? View detailed employee profiles

#### **Dashboard Analytics**
- ? Visual charts showing employee distribution
- ? Department statistics
- ? Salary analytics in ZAR

#### **Data Export**
- ? Export employee list to Excel
- ? Generate PDF reports

#### **Security Features**
- ? Role-based access (Admin vs HR)
- ? Delete permissions only for Admins
- ? Audit trail tracking

#### **API Endpoints**
- ? Test REST APIs at `/api/EmployeesApi`
- ? JSON responses for integration

### ?? **Demo Scenario**

1. **Login** as admin
2. **View Dashboard** - See employee statistics
3. **Browse Employees** - Use search and filters
4. **Add New Employee** - Upload a photo
5. **Check Audit Trail** - See tracked changes
6. **Export Data** - Download Excel/PDF
7. **Test API** - Visit `/api/EmployeesApi` in browser

### ?? **Business Value Highlights**

- **Professional UI** - Modern, responsive design
- **South African Focus** - ZAR currency, local market
- **Enterprise Features** - Audit trail, role security
- **Integration Ready** - REST APIs for other systems
- **Scalable Architecture** - Clean code, good practices

---

## Technical Stack Summary

| Component | Technology |
|-----------|------------|
| **Framework** | ASP.NET Core 8 |
| **Database** | SQL Server + Entity Framework |
| **Frontend** | Bootstrap 5 + Chart.js |
| **Security** | ASP.NET Core Identity |
| **APIs** | RESTful JSON endpoints |
| **Reports** | Excel (ClosedXML) + PDF (QuestPDF) |

---

**?? This system demonstrates full-stack .NET development skills suitable for junior to mid-level positions in South Africa.**