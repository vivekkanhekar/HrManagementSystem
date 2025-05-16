using System.ComponentModel.DataAnnotations.Schema;

namespace HrManagementSystem.Models
{
    public class AppraisalResponse
    {
       
            public int Id { get; set; }

        [ForeignKey("AppraisalTemplateId")]
        public AppTemplateLatest Template { get; set; }
        public int AppraisalTemplateId { get; set; }

        [ForeignKey("EmployeeId")]
        public string EmployeeId { get; set; }
        
        public User Employee { get; set; }

        //public string SelfAssessment { get; set; }
        //public virtual List<SelfAssessment> Entries { get; set; }

        public string KeyEntriesCsv { get; set; } // Stored in DB

        [NotMapped]
        public IList<string> KeyEntries
        {
            get => KeyEntriesCsv?.Split(';')?.ToList() ?? new List<string>();
            set => KeyEntriesCsv = string.Join(";", value);
        }

        public DateTime SubmittedDate { get; set; }
        //public string SelfAssessment { get; internal set; }
    }
    //public class SelfAssessment
    //{
    //    public int Id { get; set; }
    //    public int AppraisalSubmissionId { get; set; }
    //    public string MeasuringKey { get; set; }
    //    public int Rating { get; set; } // Out of 10

    //    public virtual AppraisalResponse Submission { get; set; }
    //}
}

