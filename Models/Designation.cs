namespace HrManagementSystem.Models
{
    public class Designation
    {
        public int DesignationId { get; set; }
        public string Title { get; set; }

        public ICollection<EmployeeOffDetails> Employees { get; set; }
    }
}
