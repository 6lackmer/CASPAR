using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    public class Section
    {
        [Key]
        public int Id { get; set; }

        #region Add Foreign Key(s)

        [Required]
        [Display(Name = "Semester")]
        public int SemesterInstanceId { get; set; }

        [ForeignKey("SemesterInstanceId")]
        public SemesterInstance? SemesterInstance { get; set; }

        [Required]
        [Display(Name = "Course")]
        public int CourseId { get; set; }

        [ForeignKey("CourseId")]
        public Course? Course { get; set; }

        [Display(Name = "Instructor")]
        public int? InstructorId { get; set; }

        [ForeignKey("InstructorId")]
        public Instructor? Instructor { get; set; }

        [Display(Name = "Modality")]
        public int? ModalityId { get; set; }

        [ForeignKey("ModalityId")]
        public Modality? Modality { get; set; }

        [Display(Name = "Classroom")]
        public int? ClassroomId { get; set; }

        [ForeignKey("ClassroomId")]
        public Classroom? Classroom { get; set; }

        [Display(Name = "Part-of-Term")]
        public int? PartOfTermId { get; set; }

        [ForeignKey("PartOfTermId")]
        public PartOfTerm? PartOfTerm { get; set; }

        [Required]
        [Display(Name = "Section Status")]
        public int SectionStatusId { get; set; }

        [ForeignKey("SectionStatusId")]
        public SectionStatus? SectionStatus { get; set; }

        [Display(Name = "Section Time")]
        public int? SectionTimeId { get; set; }

        [ForeignKey("SectionTimeId")]
        public SectionTime? SectionTime { get; set; }
        #endregion

        #region Add Fields
        public string DaysOfWeek { get; set; } = string.Empty;

        public int MaxEnrollment { get; set; } = 30;

        /*
        public int SectionWeek1Enrollment { get; set; }

        public int SectionWeek3Enrollment { get; set; }
        */

        public int CurrentEnrollment { get; set; }
        public int Waitlist { get; set; }
        public int FinalEnrollment { get; set; }

        public string? SectionNotes { get; set; }

        public int BannerCRN { get; set; }

        public DateTime SectionUpdated { get; set; }

        public DateTime SectionBannerUpdated { get; set; }

        public virtual ICollection<SectionPay>? SectionPays { get; set; }
        #endregion
    }
}