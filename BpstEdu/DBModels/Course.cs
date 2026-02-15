using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace BpstEdu.DBModels
{
    public class Course
    {
        [Key]
        public int UniqueId { get; set; }

        [Required(ErrorMessage = "Course Name is required.")]
        [StringLength(100, ErrorMessage = "Course Name cannot exceed 100 characters.")]
        public required string CourseName { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string ?Description { get; set; }





        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
         public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }
}

