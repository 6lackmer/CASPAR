using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace Infrastructure.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        #region Add Foreign Key(s)
        [Required]
        [Display(Name = "Program")]
        public int ProgramId { get; set; }

        [ForeignKey("ProgramId")]
        public virtual AcademicProgram? Program { get; set; }
        #endregion

        #region Add Fields
        [Required]
        [DisplayName("Course Title")]
        public string? CourseTitle { get; set; }

        [Required]
        [DisplayName("Credit Hours")]
        public int CreditHours { get; set; }

		[Required]
        [DisplayName("Course Number")]
        public string? CourseNumber { get; set; }

        [Required]
        [DisplayName("Course Description")]
        public string? CourseDescription { get; set; }

        public bool IsDisabled { get; set; } = false;

        public virtual ICollection<Section>? Sections { get; set; }
        #endregion
    }
}