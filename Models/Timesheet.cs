using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace HrManagementSystem.Models
{



    public class Timesheet
    {
        [Key] // This is the primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TimesheetId { get; set; }
        public string EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public User Employee { get; set; }

        [Required]
        [ForeignKey("ClientId")]
        public string ClientId { get; set; } // <-- FK property

        public User Client { get; set; }
        public string ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Projects Project { get; set; }
        public int ActivityId { get; set; }
        [ForeignKey("ActivityId")]
        public ActivityTasks Activity { get; set; }

        public DateTime Date { get; set; }

        public decimal HoursWorked { get; set; }

        public string? Description { get; set; }
        public bool? approval { get; set; }
        public string? DisapprovalReason { get; set; }


    }



}
