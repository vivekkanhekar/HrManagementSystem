using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HrManagementSystem.Models
{
    public class EmployeeLoan
    {
        [Key]
        public int LoanId { get; set; }

        [Required]
        public string EmployeeId { get; set; } // FK to AspNetUsers

        [Required]
        public decimal LoanAmount { get; set; }

        [Required]
        public decimal EMI { get; set; }

        public int TenureMonths { get; set; } // Optional, total months for repayment

        public decimal BalanceAmount { get; set; } // Remaining balance to be paid

        [Required]
        public DateTime LoanStartDate { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey("EmployeeId")]
        public virtual User Employee { get; set; }
    }

}
