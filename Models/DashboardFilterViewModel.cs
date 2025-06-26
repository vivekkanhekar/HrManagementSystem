namespace HrManagementSystem.Models
{
    public class DashboardFilterViewModel
    {
        public string SearchTerm { get; set; }
        public string SelectedClient { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string SelectedEmployeeId { get; set; }

        public List<string> Clients { get; set; }
        public List<string> EmployeeIds { get; set; }

        public IEnumerable<AppTemplateLatest> TimesheetResults { get; set; }
        public IEnumerable<AppraisalResponse> AppraisalResults { get; set; }

    }
}
