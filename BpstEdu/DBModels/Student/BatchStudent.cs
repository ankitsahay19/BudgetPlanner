using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using BpstEdu.DBModels;

namespace BpstEdu.DBModels.Student 
{
    public class BatchStudent
    {

        [Key]
        public int UniqueId { get; set; }


        [Display(Name = "Batch")]
        public int BatchId { get; set; }
        [ForeignKey(nameof(BatchId))]
        public Batch? Batch { get; set; }


        [Display(Name = "Student")]
        public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public Student? Student { get; set; }



        [Display(Name = "Discounted Fee Amount")]
        public int DiscountedFeeAmount { get; set; }


        [NotMapped]
        [Display(Name = "Total Fees")]
        public int TotalFees { get { return Batch != null ? Batch.Fees : 0; } }

        [Display(Name = "Total Fees After Discount")]
        public int TotalFeesAfterDiscount { get { return TotalFees - DiscountedFeeAmount; } }
        public List<StudentFee>? SubmittedFeeList { get; set; }

        [NotMapped]
        [Display(Name = "Remaining Fees")]
        public int RemainingFees
        {
            get
            {
                int _remainingFees = TotalFeesAfterDiscount;
                if (SubmittedFeeList != null && SubmittedFeeList.Count > 0)
                    foreach (var fee in SubmittedFeeList)
                    {
                        _remainingFees -= fee.SubmittedFeeAmount;
                    }
                return _remainingFees;
            }
        }

        public DateTime? CreatedDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }

    }
}
