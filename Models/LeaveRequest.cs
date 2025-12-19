using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Models
{
    public class LeaveRequest
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int EmployeeId { get; set; }
        public User User { get; set; }

        [Required]
        public string LeaveType { get; set; } // "Sick", "Casual", "Annual"

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public string Reason { get; set; }

        public string Status { get; set; } = "Pending"; // "Pending", "Approved", "Rejected"

        public DateTime AppliedAt { get; set; } = DateTime.Now;
    }
}
