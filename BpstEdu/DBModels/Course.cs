using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BpstEdu.Models
{
    public class Course
    {
        [Key]
        public int UniqueId { get; set; }
        [Required]
        public string Name { get; set; }
        
        public bool IntershipAvailable { get; set; }
    }
}
