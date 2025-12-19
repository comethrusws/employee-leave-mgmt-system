using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using System.Security.Claims;

namespace EmployeeManagementSystem.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;

        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }

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

        public async Task<IActionResult> MyLeaves()
        {
            int userId = int.Parse(User.FindFirst("UserId").Value);
            var leaves = await _context.LeaveRequests
                .Where(l => l.EmployeeId == userId)
                .OrderByDescending(l => l.AppliedAt)
                .ToListAsync();
            return View(leaves);
        }

        [HttpGet]
        public IActionResult ApplyLeave()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApplyLeave(LeaveRequest leave)
        {
            // Requirement says "Validate all forms". 
            // Model validation is built-in but we should check dates.
            if (leave.EndDate < leave.StartDate)
            {
                ModelState.AddModelError("EndDate", "End Date must be after Start Date");
            }

            // Remove User/EmployeeId from validation as they are set server-side
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
    }
}
