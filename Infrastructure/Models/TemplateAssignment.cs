using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    public class TemplateAssignment
    {
        [Key]
        public int Id { get; set; }

        #region Add Foreign Key(s)
        [Required]
        public int TemplateId { get; set; }

        [ForeignKey("TemplateId")]
        public Template? Template { get; set; }

        [Required]
        [Display(Name = "Course")]
        public int CourseId { get; set; }

        [ForeignKey("CourseId")]
        public Course? Course { get; set; }
        #endregion

        #region Add Fields
        [Required]
        [Display(Name = "Quantity: (1-10)")]
        [Range(1, 10, ErrorMessage = "Course Allotments must be between 1 and 10 sections")]
        public int TotalCount { get; set; }

        // Modality Assignment Counts
        [Display(Name = "Face-To-Face Quantity")]
        public int FaceToFaceCount { get; set; } = 0;

        [Display(Name = "Virtual Quantity")]
        public int VirtualCount { get; set; } = 0;

        [Display(Name = "Online Quantity")]
        public int OnlineCount { get; set; } = 0;

        [Display(Name = "FLEX Quantity")]
        public int FlexCount { get; set; } = 0;

        [Display(Name = "Hybrid Quantity")]
        public int HybridCount { get; set; } = 0;

        public int UnassignedCount { get { return TotalCount - (FaceToFaceCount + VirtualCount + OnlineCount + FlexCount + HybridCount); } }
        #endregion
    }
}
