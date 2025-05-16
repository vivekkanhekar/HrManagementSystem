using Microsoft.AspNetCore.Mvc.Rendering;

namespace HrManagementSystem.Models
{
    public class TimesheetEntryViewModel
    {
        public DateTime Date { get; set; }
        public string ClientId { get; set; }
        public string ProjectId { get; set; }
        public int ActivityId { get; set; }
        public decimal HoursWorked { get; set; }
        public string? Description { get; set; }

        //public List<SelectListItem> Clients { get; set; }
        //public List<SelectListItem> Projects { get; set; }
        //public List<SelectListItem> Activities { get; set; }
    }

}
