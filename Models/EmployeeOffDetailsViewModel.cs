namespace HrManagementSystem.Models
{
    public class EmployeeOffDetailsViewModel
    {
        public EmployeeOffDetails Details { get; set; }
        public List<User> Employees { get; set; }
        public List<JobType> JobTypes { get; set; }
        public List<Designation> Designations { get; set; }
        public List<LeaveType> LeaveTypes { get; set; }
        public List<LeaveAllocationViewModel> SelectedLeaveTypes { get; set; } // Each one has IsSelected, OpeningBalance, AccruedLeave
        public List<AssetType> AssetTypes { get; set; }
        public List<EmployeeAsset> AssignedAssets { get; set; }
        public EmployeeSalary SalaryDetails { get; set; }
        public List<SalaryComponent> SalaryComponents { get; set; }
        public List<EmployeeSalary> SalaryEntries { get; set; }
    }
}
