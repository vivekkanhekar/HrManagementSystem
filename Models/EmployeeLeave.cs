namespace HrManagementSystem.Models
{
    public class EmployeeLeave
    {
        public int Id { get; set; }
        public int EmpOffId { get; set; }
        public int LeaveTypeId { get; set; }

        public EmployeeOffDetails EmployeeOffDetails { get; set; }
        public LeaveType LeaveType { get; set; }
    }

}
