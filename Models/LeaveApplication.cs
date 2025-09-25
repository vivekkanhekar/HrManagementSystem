using System.ComponentModel.DataAnnotations;

namespace HrManagementSystem.Models
{
    public class LeaveApplication
    {
        public int Id { get; set; }

        [Required]
        public string EmployeeID { get; set; }

        public int LeaveTypeId { get; set; }

        public bool IsHalfDay { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string? Reason { get; set; }
        public string Status { get; set; } = "Pending"; //  default value
        public string? AttachmentPath { get; set; }

        public DateTime SubmittedOn { get; set; } = DateTime.Now;

        public virtual LeaveType LeaveType { get; set; }

        public virtual User Employee { get; set; }
    }
}
