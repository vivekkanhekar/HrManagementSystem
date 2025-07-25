using System.ComponentModel.DataAnnotations;

namespace HrManagementSystem.Models
{
    public class LeaveType
    {
        [Key]
        public int Leave_Id { get; set; }
        public string LeaveName { get; set; }
        public string? Description { get; set; }
        public int DaysAllowed { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<EmployeeLeave> EmployeeLeaves { get; set; }
    }

}
