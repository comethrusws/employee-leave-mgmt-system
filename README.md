# Employee Leave Management System

A robust, web-based application built with **ASP.NET Core MVC** (.NET 9) designed to streamline employee leave management. This system provides role-based access for Administrators to manage employees and leave requests, and for Employees to apply for and track their leaves.

## 🚀 Features

### 👨‍💼 Admin Module
- **Dashboard**: Real-time overview of total employees and leave request stats (Pending, Approved).
- **Employee Management**: Full CRUD capabilities to Add, Edit, and Delete employees.
- **Leave Management**: Review incoming leave requests with the ability to **Approve** or **Reject** them.

### 🧑‍💻 Employee Module
- **Dashboard**: Personal dashboard showing leave statistics (Total Applied, Approved, Rejected, Pending).
- **Apply for Leave**: easy-to-use form to submit leave applications (Sick, Casual, Annual).
- **Track Status**: View history and current status of all submitted requests.

## 🏗 System Architecture

The application follows the **Model-View-Controller (MVC)** design pattern, ensuring a clean separation of concerns.

- **Backend**: ASP.NET Core MVC (.NET 9) using C#.
- **Database**: **SQLite** via **Entity Framework Core** (Code-First approach). *Note: Switched from LocalDB for broader compatibility.*
- **Authentication**: Cookie-based Authentication with Claims Identity.
- **Session Management**: ASP.NET Core Session for temporary user data.
- **Frontend**: Razor Views (`.cshtml`) styled with **Bootstrap 5**.

### Key Components
- **Controllers**:
  - `AccountController`: Manages secure Login and Logout processes.
  - `AdminController`: Handles all administrative tasks (User management, Leave approval).
  - `EmployeeController`: Manages employee-specific actions (Applying, Dashboard).
- **Models**:
  - `User`: Entity for application users (Roles: Admin, Employee).
  - `LeaveRequest`: Entity for tracking leave details, dates, and status.
- **Data**:
  - `AppDbContext`: The bridge between the app and the SQLite database.

## 🛠 How to Run

### Prerequisites
- [.NET SDK 9.0](https://dotnet.microsoft.com/download) installed.

### Steps
1. **Clone/Navigate** to the project directory:
   ```bash
   cd EmployeeManagementSystem
   ```

2. **Restore Dependencies**:
   ```bash
   dotnet restore
   ```

3. **Run the Application**:
   ```bash
   dotnet run
   ```
   The application will start, typically on `http://localhost:5032` (check your terminal output).

4. **Access the App**: Open your browser and navigate to the localhost URL.

## 🔑 Default Credentials

The system comes seeded with a default Administrator account:

| Role  | Email | Password |
|-------|-------|----------|
| **Admin** | `admin@company.com` | `Admin@123` |

*To test the Employee flow, login as Admin and create a new Employee user.*
