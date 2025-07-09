namespace HrManagementSystem.Models
{
    public class JobType
    {
        public int JobTypeId { get; set; }
        public string Name { get; set; }

        public ICollection<EmployeeOffDetails> Employees { get; set; }
    }
}
