using System.ComponentModel.DataAnnotations;

namespace HrManagementSystem.Models
{
    public class ApplyLeaveViewModel
    {
        [Required]
        public int LeaveTypeId { get; set; }

        public string DurationType { get; set; } = "FullDay"; // "FullDay" or "HalfDay"

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? SingleDate { get; set; }

        [Required]
        public string Reason { get; set; }

        public IFormFile? Attachment { get; set; }

        public List<LeaveType> LeaveTypes { get; set; }
    }
}
