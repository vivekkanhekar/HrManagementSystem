
using System.ComponentModel.DataAnnotations.Schema;

namespace HrManagementSystem.Models
{
    public class Approval
    {
        public int ApprovalID { get; set; }
       
        public string ApprovedBy { get; set; }
        public string ApprovalStatus { get; set; } = "Pending";
        public DateTime? ApprovedAt { get; set; }
        //[ForeignKey("Timesheet")]
        //public int TimesheetId { get; set; }
        //public virtual Timesheet Timesheet { get; set; }

        [ForeignKey("ApprovedByEmpId")]
        public User ApprovedByEmp { get; set; }
    }
}
