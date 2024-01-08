using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    public class TimeBlock : IComparable<TimeBlock>
    {
        [Key]
        public int Id { get; set; }

        #region Add Foreign Key(s)
        [Required]
        public int TimeBlockTypeId { get; set; }

        [ForeignKey("TimeBlockTypeId")]
        public TimeBlockType? TimeBlockType { get; set; }
        #endregion

        #region Add Fields
        [Required]
        [Display(Name = "Time Block Name")]
        public string? TimeBlockName { get; set; } // Early Morning (7:30-9:00 AM), etc.

        [Required]
        [Display(Name = "Start Time")]
        public TimeSpan StartTime { get; set; }

        [Required]
        [Display(Name = "End Time")]
        public TimeSpan EndTime { get; set; }

        public bool IsDisabled { get; set; } = false;
        #endregion

        #region Navigation Propety
        public virtual ICollection<InstructorWishlistCourse>? InstructorWishlistCourses { get; set; }
        public virtual ICollection<StudentWishlistCourse>? StudentWishlistCourse { get; set; }
        #endregion

        public int CompareTo(TimeBlock? other)
        {
            return Id.CompareTo(other?.Id);
        }
    }
}
