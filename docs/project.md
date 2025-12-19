# PROJECT REPORT: EMPLOYEE LEAVE MANAGEMENT SYSTEM

## 1. INTRODUCTION
### 1.1. OVERVIEW
The Employee Leave Management System is a web-based application developed to streamline the process of leave management within an organization. It provides a centralized platform for employees to request leaves and for administrators to manage and approve them efficiently.

### 1.2. MOTIVATION
Traditional paper-based or email-based leave tracking is prone to errors, data loss, and delays. This system aims to digitize the workflow, ensuring transparency, quick processing, and accurate record-keeping.

### 1.3. TECHNOLOGIES USED
- **Framework:** ASP.NET Core MVC (.NET 9)
- **Language:** C#
- **Database:** SQLite (via Entity Framework Core)
- **Frontend:** Razor Views, Bootstrap 5, CSS

### 1.4. FUTURE SCOPE
- Integration with Payroll systems.
- Email notifications for leave status updates.
- specialized calendar views for team availability.

### 1.5. LEARNING AND OUTCOME
- Understanding of the MVC architectural pattern.
- Practical experience with Entity Framework Core and Database management.
- Implementation of Role-Based Authentication and Authorization.

## 2. OBJECTIVES
- To create a secure and user-friendly interface for employees and admins.
- To automate the leave application and approval workflow.
- To maintain an organized record of employee data and leave history.

## 3. SYSTEM DESIGN

### 3.1. ER DIAGRAM
The schematic design of the database is crucial for understanding the relationships between different data entities.

**Entities:**
1.  **User**: Represents the application users (Admins and Employees).
    *   **Attributes**: `Id` (PK), `FullName`, `Email`, `PasswordHash`, `Role`, `CreatedAt`.
2.  **LeaveRequest**: Represents a leave application submitted by an employee.
    *   **Attributes**: `Id` (PK), `EmployeeId` (FK), `LeaveType`, `StartDate`, `EndDate`, `Reason`, `Status`, `AppliedAt`.

**Relationships:**
- **One-to-Many**: A single `User` (Employee) can have multiple `LeaveRequest` records. The `LeaveRequest` entity references the `User` entity via the `EmployeeId` foreign key.

### 3.2. GENERAL BLOCK DIAGRAM
The system operates on a request-response cycle typical of web applications.
1.  **User Interface (Frontend)**: Employees and Admins interact with the system via web browsers.
2.  **Application Logic (Backend)**: The ASP.NET Core server facilitates the flow of data.
    -   **Routing**: Maps URLs to specific Controllers.
    -   **Controllers**: Process input and interact with Models.
3.  **Data Layer**: The Entity Framework Core context manages CRUD operations against the SQLite database.

### 3.3. SYSTEM ARCHITECTURE
The system follows the **Model-View-Controller (MVC)** architectural pattern:

- **Model**: Represents the data and business logic.
    -   *Files*: `User.cs`, `LeaveRequest.cs`.
    -   These classes define the schema and validation rules for the data.
- **View**: Represents the presentation layer (UI).
    -   *Files*: `.cshtml` files in `Views/Admin`, `Views/Employee`, etc.
    -   Responsible for rendering HTML content to the user.
- **Controller**: Handles user requests, interacts with the Model, and selects a View to render.
    -   *Files*: `AdminController.cs`, `EmployeeController.cs`.
    -   Manages the flow of application execution.

## 4. IMPLEMENTATION

### 4.1. CODE ARCHITECTURE
The project is organized into logical folders to maintain separation of concerns:

-   **Controllers/**: Contains the class files handling HTTP requests.
    -   `AdminController`: Manages employee records (Create/Edit/Delete) and leave approvals.
    -   `EmployeeController`: Handles leave applications and personal dashboard views.
    -   `AccountController`: Manage authentication (Login/Logout).
-   **Models/**: Defines the data structures.
    -   `AppDbContext`: The Data Context class that manages the entity sets (`Users`, `LeaveRequests`) and handles database connectivity.
    -   Domain classes (`User`, `LeaveRequest`) annotated with DataAttributes (e.g., `[Key]`, `[Required]`).
-   **Views/**: Stores the logic for the user interface.
    -   Shared layouts (`_Layout.cshtml`) ensure consistent design across pages.
    -   View folders match Controller names (`Admin`, `Employee`).
-   **wwwroot/**: A standard folder for static assets like CSS (`site.css`), JavaScript, and images.

### 4.2. KEY IMPLEMENTATION LOGIC

This section details the core logic that drives the application's functionality.

#### 4.2.1. Authentication and Security
The system employs a secure password hashing mechanism using SHA-256. When creating a user, the raw password is hashed before being stored in the database.

**Password Hashing Logic**
```csharp
private string HashPassword(string password)
{
    using (SHA256 sha256 = SHA256.Create())
    {
        byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        StringBuilder builder = new StringBuilder();
        foreach (byte b in bytes)
        {
            builder.Append(b.ToString("x2"));
        }
        return builder.ToString();
    }
}
```

#### 4.2.2. Leave Application Workflow
The employee leave application process involves server-side validation to ensure data integrity. Critical checks include verifying that the End Date is not earlier than the Start Date.

**Leave Application Controller Action**
```csharp
[HttpPost]
public async Task<IActionResult> ApplyLeave(LeaveRequest leave)
{
    if (leave.EndDate < leave.StartDate)
    {
        ModelState.AddModelError("EndDate", "End Date must be after Start Date");
    }
    ModelState.Remove("User");
    ModelState.Remove("Status");
    if (!ModelState.IsValid) return View(leave);
    int userId = int.Parse(User.FindFirst("UserId").Value);
    leave.EmployeeId = userId;
    leave.Status = "Pending";
    leave.AppliedAt = DateTime.Now;
    _context.LeaveRequests.Add(leave);
    await _context.SaveChangesAsync();
    return RedirectToAction("MyLeaves");
}
```

#### 4.2.3. Dashboard Statistics Aggregation
The dashboard provides a real-time summary of leave requests. This is achieved by querying the database and filtering records based on the user's ID and leave status using LINQ.

**Employee Dashboard Logic**
```csharp
public async Task<IActionResult> Dashboard()
{
    int userId = int.Parse(User.FindFirst("UserId").Value);
    var leaves = await _context.LeaveRequests.Where(l => l.EmployeeId == userId).ToListAsync();
    ViewBag.Total = leaves.Count;
    ViewBag.Approved = leaves.Count(l => l.Status == "Approved");
    ViewBag.Pending = leaves.Count(l => l.Status == "Pending");
    ViewBag.Rejected = leaves.Count(l => l.Status == "Rejected");
    return View(leaves.OrderByDescending(l => l.AppliedAt).Take(5).ToList());
}
```

#### 4.2.4. Admin Approval Workflow
Administrators have the authority to process leave requests. The logic fetches the specific leave request by ID and updates its status string based on the administrator's action.

**Leave Status Update Logic**
```csharp
[HttpPost]
public async Task<IActionResult> RespondLeave(int id, string status)
{
    var leave = await _context.LeaveRequests.FindAsync(id);
    if (leave != null)
    {
        leave.Status = status;
        await _context.SaveChangesAsync();
    }
    return RedirectToAction("LeaveRequests");
}
```

## 5. TESTING AND VALIDATION

### 5.1. INTEGRATION TESTING
Integration testing focuses on verifying that different modules of the application work together as expected.

-   **Database Integration**: Verified that creating a new `LeaveRequest` in the `EmployeeController` correctly persists a record in the `LeaveRequests` table in the SQLite database.
-   **Role-Based Access Control**: Verified that users with the "Employee" role cannot access `AdminController` actions (e.g., `CreateEmployee`), ensuring the `[Authorize(Roles = "Admin")]` attribute functions correctly.
-   **Relation Checks**: Confirmed that fetching a `LeaveRequest` also correctly retrieves the associated `User` details using EF Core `.Include(l => l.User)`.

### 5.2. MANUAL UI TESTING
Manual testing was performed to simulate real-world user scenarios.

**Scenario 1: Employee Leave Application**
1.  Log in as an Employee.
2.  Navigate to "Apply Leave".
3.  Fill in the form with valid Start and End dates.
4.  Submit the form.
5.  **Result**: Redirected to "My Leaves" list; status shows "Pending".

**Scenario 2: Admin Approval Workflow**
1.  Log in as an Admin.
2.  Navigate to "Leave Requests".
3.  Locate the pending request from Scenario 1.
4.  Click "Approve".
5.  **Result**: Status updates to "Approved" in the database; Employee dashboard reflects the change.

### 4.1. FORM VALIDATION
(Note: Numbering follows provided structure)

Data integrity is enforced using Data Annotations in the Models and logic in the Controllers.

-   **Required Fields**: The `[Required]` attribute prevents submission of forms with missing critical data (e.g., Email, Start Date).
-   **Email Format**: The `[EmailAddress]` attribute ensures the email field follows a valid format.
-   **Business Logic Validation**:
    -   *Date Validation*: In `EmployeeController.ApplyLeave`, a check is implemented to ensure `EndDate` is not earlier than `StartDate`.
    ```csharp
    if (leave.EndDate < leave.StartDate)
    {
        ModelState.AddModelError("EndDate", "End Date must be after Start Date");
    }
    ```
    -   *Unique Email*: In `AdminController.CreateEmployee`, a check ensures that the email provided for a new employee does not already exist in the database.
