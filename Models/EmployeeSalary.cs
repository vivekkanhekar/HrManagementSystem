namespace HrManagementSystem.Models
{
    public class EmployeeSalary
    {
        public int ID { get; set; }
        public string EmployeeID { get; set; } // FK to AspNetUsers
        public decimal CTCAnnual { get; set; }
        public decimal FBP { get; set; }
        public decimal VariablePay { get; set; }

        public int ComponentID { get; set; } // FK to SalaryComponent
        public decimal MonthlyAmount { get; set; }
        public decimal AnnualAmount { get; set; }

        public virtual SalaryComponent Component { get; set; }
        public virtual User Employee { get; set; }
    }
}
