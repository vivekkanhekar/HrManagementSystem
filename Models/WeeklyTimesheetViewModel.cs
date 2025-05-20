using Microsoft.AspNetCore.Mvc.Rendering;

namespace HrManagementSystem.Models
{
    public enum ApprovalStatus
    {
        Pending,
        Approved,
        Disapproved
    }

    public class WeeklyTimesheetViewModel
    {
        public string EmployeeId { get; set; }
        public List<DailyTimesheetEntry> DailyEntries { get; set; }
        public List<SelectListItem> Clients { get; set; }
        public List<SelectListItem> Projects { get; set; }
        public List<SelectListItem> ActivityTasks { get; set; }
        public List<SelectListItem> ManagerID { get; set; }

    }
    public class DailyTimesheetEntry
    {
        public DateTime Date { get; set; }
        public string ClientId { get; set; }

        public string ManagerID { get; set; }
        public string ProjectId { get; set; }
        public int ActivityId { get; set; }
        public decimal HoursWorked { get; set; }
        public string? Description { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; } = ApprovalStatus.Pending;
        public string DisapprovalReason { get; set; }

    }
}
