using Microsoft.AspNetCore.Mvc.Rendering;

namespace HrManagementSystem.Models
{
    public class ManageLeaveFilterViewModel
    {
        public List<EmployeeLeave> EmployeeLeave { get; set; }

        public List<SelectListItem> Employees { get; set; }
        public List<SelectListItem> LeaveTypes { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? SelectedEmployeeId { get; set; }
        public int? SelectedLeaveTypeId { get; set; }
    }

}
