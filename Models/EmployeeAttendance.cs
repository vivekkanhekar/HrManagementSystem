using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrManagementSystem.Models
{
    public class EmployeeAttendance
    {
        [Key]
        public int AttendanceId { get; set; }

        [Required]
        public string EmployeeId { get; set; }   // FK to AspNetUsers (Employee)

        [ForeignKey("EmployeeId")]
        public virtual User Employee { get; set; }

        [Required]
        public DateTime AttendanceDate { get; set; } = DateTime.Now.Date;

        public DateTime? PunchInTime { get; set; }

        public DateTime? PunchOutTime { get; set; }

        public decimal? TotalHours { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "Present";   // Present, Absent, Half-Day, Incomplete

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
