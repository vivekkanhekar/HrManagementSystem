using HrManagementSystem.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrManagementSystem.Models
{
    public class AttendanceCorrection
    {
        [Key]
        public int CorrectionId { get; set; }

        [Required]
        public int AttendanceId { get; set; }   // FK to EmployeeAttendance

        [ForeignKey("AttendanceId")]
        public virtual EmployeeAttendance Attendance { get; set; }

        public DateTime? RequestedPunchIn { get; set; }
        public DateTime? RequestedPunchOut { get; set; }


        public string? Reason { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected

        public DateTime RequestedDate { get; set; } = DateTime.Now;
    }
}
