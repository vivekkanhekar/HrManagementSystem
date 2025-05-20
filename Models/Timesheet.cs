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

        [Required]
        public string EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public User Employee { get; set; }

        [Required]
        public string? ManagerID { get; set; }
        [ForeignKey("ManagerID")]

        public User? Manager { get; set; }

        [Required]
        public string ClientId { get; set; } // <-- FK property
        [ForeignKey("ClientId")]


        public User Client { get; set; }

        [Required]
        [ForeignKey("ProjectId")]
        public string ProjectId { get; set; }
        public Projects Project { get; set; }

        [Required]
        [ForeignKey("ActivityId")]
        public int ActivityId { get; set; }
        public ActivityTasks Activity { get; set; }

        public DateTime Date { get; set; }

        public decimal HoursWorked { get; set; }

        public string? Description { get; set; }
        public bool? approval { get; set; }
        public string? DisapprovalReason { get; set; }


    }



}
