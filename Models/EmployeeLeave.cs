namespace HrManagementSystem.Models
{
    public class EmployeeLeave
    {
        public int Id { get; set; }
        public string EmpOffId { get; set; }
        public int LeaveTypeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; } // Pending, Approved, Rejected
        public EmployeeOffDetails EmployeeOffDetails { get; set; }
        public LeaveType LeaveType { get; set; }
    }

}
