using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace HrManagementSystem.Models
{
    public class Projects
    {
        [Key]
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        //public string ActivityId { get; set; }
        //[ForeignKey("ActivityId")]
        //public ActivityTasks Activity { get; set; }
    }

}
