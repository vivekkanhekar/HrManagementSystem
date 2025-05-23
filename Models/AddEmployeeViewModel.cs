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
        [Required]

        public string Client { get; set; }

        [Required]
        public string Phone { get; set; }

        public string? Address { get; set; }
    }

}
