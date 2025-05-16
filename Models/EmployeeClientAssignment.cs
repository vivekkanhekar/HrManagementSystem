using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrManagementSystem.Models
{
    public class EmployeeClientAssignment
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("EmployeeId")]
        public string EmployeeId { get; set; }
        public User Employee { get; set; }

        [Required]
        [ForeignKey("ClientId")]
        public string ClientId { get; set; }
        public User Client { get; set; }
        public string? FilePath { get; set; }
        public string? FileName { get; set; }

        [Required]
        [ForeignKey("ManagerId")]
        public User Manager { get; set; }
        public string ManagerId { get; set; }
        public DateTime AssignedOn { get; set; } = DateTime.Now;

    }
}
