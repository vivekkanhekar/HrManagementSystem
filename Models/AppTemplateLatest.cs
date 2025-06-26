using System.ComponentModel.DataAnnotations.Schema;

namespace HrManagementSystem.Models
{
    public class AppTemplateLatest
    {

        public int Id { get; set; }

        [ForeignKey("ClientId")]
        public string ClientId { get; set; }
        public User Client { get; set; }

        [ForeignKey("DepartmentId")]

        public Department Department { get; set; }

        public int DepartmentId { get; set; }

        [ForeignKey("ActivityId")]

        public int ActivityId { get; set; }
        public ActivityTasks Activity { get; set; }
        public string? FilePath { get; set; }
        public string? FileName { get; set; }
        public List<string> MeasuringKeys { get; set; }

        public List<string> Remarks { get; set; }
        public List<string> Amount { get; set; }

        [ForeignKey("ManagerId")]
        public User Manager { get; set; }
        public string ManagerId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
}
