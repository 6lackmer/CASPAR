using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class Instructor
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser? ApplicationUser { get; set; }

        #region Add Fields
        [Display(Name = "Title")]
        public string? InstructorTitle { get; set; }

		public bool IsDisabled { get; set; } = false;

		public virtual ICollection<AcademicProgram>? ProgramAffiliation { get; set; }

        public virtual ICollection<InstructorWishlist>? InstructorWishlists { get; set; }
        #endregion
    }
}
