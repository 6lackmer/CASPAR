using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    public class InstructorWishlist
    {
        [Key]
        public int Id { get; set; }

        #region Add Foreign Key(s)
        [Required]
        [Display(Name = "Semester Instance")]
        public int SemesterInstanceId { get; set; }

        [ForeignKey("SemesterInstanceId")]
        public SemesterInstance? SemesterInstance { get; set; }

        [Required]
        [Display(Name = "Instructor")]
        public int InstructorId { get; set; }

        [ForeignKey("InstructorId")]
        public Instructor? Instructor { get; set; }
        #endregion
    }
}
