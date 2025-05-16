using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace HrManagementSystem.Models
{
    public class SuperAdmin
    {
        public string id;

        [Required]

        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and Confirm password must match!.")]
        public string ConfirmPassword { get; set; }

        [Required]

        public Role Role { get; set; }
    }
}
