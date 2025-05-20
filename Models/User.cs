
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HrManagementSystem.Models
{
    public class User: IdentityUser
    {

      
        public string FirstName { get; set; }
        //public string UserName { get; set; }
        public string? Address { get; set; }

        public string? LastName { get; set; }
        //public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        //public int Phone { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        //public virtual ICollection<ClientAdmin> ClientAdmins { get; set; } = new List<ClientAdmin>();
        // public ICollection<Department> Departments { get; set; }
        //List<TimesheetEmployee> TimesheetEmployees { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; } = true;
        //public ICollection<Timesheet> Timesheets { get; set; }
        
    }
}
