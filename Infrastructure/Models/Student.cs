using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }

        #region Add Foreign Key(s)
        [Required]
        [Display(Name = "User")]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }

        [Display(Name = "Major")]
        public int? MajorId { get; set; }

        [ForeignKey("MajorId")]
        public Major? Major { get; set; }
		#endregion

		#region Add Fields

		public bool IsDisabled { get; set; } = false;

        [Display(Name = "Graduation Year")]
		public int GraduationYear { get; set; }

		public virtual ICollection<StudentWishlist>? StudentWishlists { get; set; }
		#endregion
	}
}
