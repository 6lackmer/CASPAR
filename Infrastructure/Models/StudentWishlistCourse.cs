using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    public class StudentWishlistCourse
    {
        [Key]
        public int Id { get; set; }

        #region Add Foreign Key(s)
        [Required]
        [Display(Name = "Student Wishlist Id")]
        public int StudentWishlistId { get; set; }

        [ForeignKey("StudentWishlistId")]
        public StudentWishlist? StudentWishlist { get; set; }

        [Required]
        [Display(Name = "Course")]
        public int CourseId { get; set; }

        [ForeignKey("CourseId")]
        public Course? Course { get; set; }
        #endregion

        #region Add Fields 
        public virtual ICollection<Modality>? Modalities { get; set; }

        public virtual ICollection<Campus>? Campuses {  get; set; }

        public virtual ICollection<TimeBlock>? TimeBlocks { get; set; }
        #endregion
    }
}
