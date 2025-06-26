using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HrManagementSystem.Models
{
    public class ClientLeavePolicy
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Client")]
        public string ClientID { get; set; }

        public int CasualLeaves { get; set; }
        public int SickLeaves { get; set; }
        public int MaternityLeaves { get; set; }

        public int PrivilegedLeaves { get; set; }

        public virtual User Client { get; set; }
    }
}
