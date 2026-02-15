using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BpstEdu.DBModels.Student
{
    public class StudentFee
    {
        [Key]
        public int UniqueId { get; set; } 
        public int BatchStudentId { get; set; }
        public int StudentId { get; set; } 
        public int SubmittedFeeAmount { get; set; }
        public string ?Description { get; set; }
        public DateTime FeeSubmittingDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }  
    }
}
