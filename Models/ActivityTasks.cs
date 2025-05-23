using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HrManagementSystem.Models
{

    public class ActivityTasks
    {

        
        [Key]
        public int ActivityId { get; set; }
        //public string? OperationName { get; set; }
        public string ActivityName { get; set; }
        public string? Description { get; set; }
        //public ActivityTasks(string operationName)
        //{
        //    OperationName = operationName;
        //}

        [ForeignKey("ProjectId")]
        //public ActivityTasks ProjectId { get; set; }
        public string ProjectId { get; set; }
        public Projects Project { get; set; }
    }


}
