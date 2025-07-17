using System.ComponentModel.DataAnnotations;

namespace HrManagementSystem.Models
{
    public class EmployeeAsset
    {
        [Key]
        public int ID { get; set; }
        public string EmployeeID { get; set; } // FK to AspNetUsers
        public int AssetTypeID { get; set; } // FK to AssetType
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public virtual AssetType AssetType { get; set; }
        public virtual User Employee { get; set; }
    }

}

