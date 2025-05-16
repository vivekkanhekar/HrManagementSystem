using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HrManagementSystem.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [AllowNull]
        public string? Description { get; set; }
    }
}
