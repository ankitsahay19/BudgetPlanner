using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
 
namespace BpstEdu.DBModels
{
    public class Batch
    {
        [Key]
        public int UniqueId { get; set; }


        public required Course Course { get; set; }
        [ForeignKey("Course")]
        public int? CourseId { get; set; }

        public int Fees { get; set; }
        public int TenureInDays { get; set; }

        public DateTime StartingFrom { get; set; }

        [NotMapped]
        public DateTime EndDate { get { return StartingFrom.AddDays(TenureInDays); } } 

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }
}

