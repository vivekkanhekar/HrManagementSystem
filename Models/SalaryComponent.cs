using System.ComponentModel.DataAnnotations;

namespace HrManagementSystem.Models
{
    public class SalaryComponent
    {
        [Key]
        public int ComponentID { get; set; }
        public string ComponentName { get; set; }
        public string Formula { get; set; }
        public bool IsCTCBased { get; set; }
        public bool IsActive { get; set; }
    }


}
