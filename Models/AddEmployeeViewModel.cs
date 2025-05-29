using System.ComponentModel.DataAnnotations;

namespace HrManagementSystem.Models
{
    public class AddEmployeeViewModel
    {
       // public string Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string? LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
        public string? Department { get; set; }
        
        public string? Client { get; set; }

        
        public string? Phone { get; set; }

        public string? Address { get; set; }
    }

}
