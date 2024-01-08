using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
	public class StudentWishlist
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
		[Display(Name = "Student")]
		public int StudentId { get; set; }

		[ForeignKey("StudentId")]
		public Student? Student { get; set; }
		#endregion
	}
}
