using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    public class InstructorWishlistCourse
    {
        [Key]
        public int Id { get; set; }

        #region Add Foreign Key(s)
        [Required]
        [Display(Name = "Instructor Wishlist Id")]
        public int InstructorWishlistId { get; set; }

        [ForeignKey("InstructorWishlistId")]
        public InstructorWishlist? InstructorWishlist { get; set; }

        [Required]
        [Display(Name = "Course")]
        public int CourseId { get; set; }

        [ForeignKey("CourseId")]
        public Course? Course { get; set; }
        #endregion

        #region Add Fields 
        [Required]
        public int Ranking { get; set; }

        public string DaysOfWeek { get; set; } = string.Empty;

        public virtual ICollection<Modality>? Modalities { get; set; }

        public virtual ICollection<Campus>? Campuses { get; set; }

        public virtual ICollection<TimeBlock>? TimeBlocks { get; set; }
        #endregion
    }
}
