using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrManagementSystem.Models
{
    public class EmpManager
    {
        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string Phone { get; set; }
        public string? Address { get; set; }

        [Key]
        public int Id { get; set; }


        //[ForeignKey("ClientAdmin")]
        //public int ClientAdminId { get; set; }
        //public virtual ClientAdmin ClientAdmin { get; set; }


        [ForeignKey("User")]
        public string? clientId { get; set; }
        public virtual User Client { get; set; }
    }

}
