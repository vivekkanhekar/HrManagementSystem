using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrManagementSystem.Models
{
    public class TimesheetEmployee
    {
        [Key]
        public int timeemp_Id { get; set; }

        [Required]
        [ForeignKey("Client")]
        public string ClientId { get; set; }
        public virtual User Client { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User User { get; set; }

        [Required]
        [ForeignKey("Depart")]
        public int DeparId { get; set; }
        public virtual Department Depart { get; set; }

        [Required]
        [ForeignKey("Manager")]
        public string AdminId { get; set; }
        public virtual User Admin { get; set; }

    }
}
