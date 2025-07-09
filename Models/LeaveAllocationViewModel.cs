namespace HrManagementSystem.Models
{
    public class LeaveAllocationViewModel
    {
        public int Leave_Id { get; set; }
        public bool IsSelected { get; set; }
        public int OpeningBalance { get; set; }
        public int AccruedLeave { get; set; }
    }
}
