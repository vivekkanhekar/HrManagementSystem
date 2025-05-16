namespace HrManagementSystem.Models
{
    public class Manager 
    {
        public int Id { get; set; } // Primary Key
        public string? Name { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }

        // Navigation Property (if Managers oversee Employees)
        public ICollection<User> Employees { get; set; }
    }

}
