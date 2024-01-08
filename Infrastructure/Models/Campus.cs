using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    public class Campus : IComparable<Campus>
    {
        [Key]
        public int Id { get; set; }

        #region Add Fields
        [Required]
        [Display(Name = "Campus Name")]
        public string? CampusName { get; set; }

        public bool IsDisabled { get; set; } = false;
        #endregion

        #region Navigation Propety
        public virtual ICollection<InstructorWishlistCourse>? InstructorWishlistCourses { get; set; }
        public virtual ICollection<StudentWishlistCourse>? StudentWishlistCourse { get; set; }
        #endregion

        public int CompareTo(Campus? other)
        {
            return Id.CompareTo(other?.Id);
        }
    }
}
