using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    public class Modality : IComparable<Modality>
    {
        [Key]
        public int Id { get; set; }

        #region Fields
        [Required]
        [Display(Name = "Modality Name")]
        public string? ModalityName { get; set; }

        public string? Description { get; set; }

        [Required]
        [Display(Name = "Has Campuses", Description = "Has a physical meeting location.")]
        public bool HasCampuses { get; set; } = true;

        [Required]
        [Display(Name = "Has Time Blocks", Description = "Has a set meeting time.")]
        public bool HasTimeBlocks { get; set; } = true;

        public bool IsDisabled { get; set; } = false;
        #endregion

        #region Navigation Property
        public virtual ICollection<InstructorWishlistCourse>? InstructorWishlistCourses { get; set; }
        public virtual ICollection<StudentWishlistCourse>? StudentWishlistCourse { get; set; }
        #endregion

        public int CompareTo(Modality? other)
        {
            return Id.CompareTo(other?.Id);
        }
    }
}
