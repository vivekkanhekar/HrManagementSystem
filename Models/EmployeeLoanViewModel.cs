namespace HrManagementSystem.Models
{
    public class EmployeeLoanViewModel
    {
        public EmployeeLoan Loan { get; set; }
        public List<EmployeeLoan> LoanList { get; set; }
        public string SearchTerm { get; set; }
        public bool? FilterActive { get; set; }
    }
}
