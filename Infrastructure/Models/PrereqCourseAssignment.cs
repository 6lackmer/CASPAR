using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
	public class PrereqCourseAssignment
	{
		[Key]
		public int Id { get; set; }

		#region Add Foreign Key(s)
		[Required]
		[Display(Name = "Course")]
		public int CourseId { get; set; }

		[ForeignKey("CourseId")]
		public Course? Course { get; set; }

		[Display(Name = "Prerequisite Course")]
		public int? PrereqCourseId { get; set; }

		[ForeignKey("PrereqCourseId")]
		public Course? PrerequisiteCourse { get; set; }
        #endregion
    }
}
