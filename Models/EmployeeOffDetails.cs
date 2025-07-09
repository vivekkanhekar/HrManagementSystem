using System.ComponentModel.DataAnnotations;

namespace HrManagementSystem.Models
{
    public class EmployeeOffDetails
    {
        public int Id { get; set; }

        [Required]
        public string EmployeeId { get; set; } // FK to AspNetUsers

        [Required]
        public string PAN { get; set; }

        [Required]
        public DateTime JoiningDate { get; set; }

        [Required]
        public string AccountNumber { get; set; }

        public int JobTypeId { get; set; }
        public int DesignationId { get; set; }
        public bool IsBillable { get; set; }

        public virtual JobType JobType { get; set; }
        public virtual Designation Designation { get; set; }
        public virtual User Employee { get; set; }
        public ICollection<EmployeeLeave> EmployeeLeaves { get; set; }

        // PF Fields
        public bool HasPF { get; set; }
        public string? PF_UAN { get; set; }
        public string? PFNumber { get; set; }
        public DateTime? PFEnrollmentDate { get; set; }
        public string? EPFNumber { get; set; }

        // ESI Fields
        public bool HasESI { get; set; }
        public string? ESINumber { get; set; }
        public DateTime? ESIEnrollmentDate { get; set; }


    }
}
