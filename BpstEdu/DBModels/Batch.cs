using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
 
namespace BpstEdu.DBModels
{
    public class Batch
    {
        [Key]
        public int UniqueId { get; set; }


        // Navigation property is nullable so model binding doesn't require a full Course object
        public Course? Course { get; set; }
        [ForeignKey("Course")]
        public int? CourseId { get; set; }

        public int Fees { get; set; }
        [DisplayName("Tenure (in Day's)")]
        public int TenureInDays { get; set; }

        [DisplayName("Batch Start's From ")]
        public DateTime StartingFrom { get; set; }

        [NotMapped]
        public DateTime EndDate { get { return StartingFrom.AddDays(TenureInDays); } } 

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime LastUpdatedDate { get; set; }
    }
}

