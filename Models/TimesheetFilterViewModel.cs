namespace HrManagementSystem.Models
{
    public class TimesheetFilterViewModel
    {
        public List<TimesheetEntryViewModel> Timesheets { get; set; }
        public string SelectedClient { get; set; }
        public string SelectedProject { get; set; }
        public string SelectedManager { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public List<string> AvailableClients { get; set; }
        public List<string> AvailableProjects { get; set; }
        public List<string> AvailableManagers { get; set; }

    }
}
