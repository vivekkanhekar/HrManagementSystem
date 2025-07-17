using System.ComponentModel.DataAnnotations;

namespace HrManagementSystem.Models
{
    public class AssetType
    {
        [Key]
        public int ID { get; set; }
        public string AssetName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
    }

}
