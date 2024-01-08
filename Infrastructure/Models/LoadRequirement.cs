using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    public class LoadRequirement
    {
        [Key]
        public int Id { get; set; }

        #region Add Foreign Key(s)
        [Required]
        [Display(Name = "Instructor")]
        public int InstructorId { get; set; }

        [ForeignKey("InstructorId")]
        public Instructor? Instructor{ get; set; }

        [Required]
        [Display(Name = "Semester")]
        public int SemesterInstanceId { get; set; }

        [ForeignKey("SemesterInstanceId")]
        public SemesterInstance? SemesterInstance { get; set; }
        #endregion

        #region Add Fields
        [Required]
        [Display(Name = "Required Load Hours")]
		[Range(1, 12, ErrorMessage = "Load Hours Required Shouldn't Exceed 12")]
		public int LoadReqHours { get; set; }
        #endregion
    }
}
