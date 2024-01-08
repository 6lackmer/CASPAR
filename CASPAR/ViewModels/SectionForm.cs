using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.ComponentModel.DataAnnotations;
using Utility.Models;

namespace CASPAR.ViewModels
{
    public class SectionForm
    {
        [Required]
        public int Id { get; set; }

        [Display(Name = "Instructor")]
        public int? InstructorId { get; set; }

        [Display(Name = "Modality")]
        public int? ModalityId { get; set; }

        [Display(Name = "Campus")]
        public int? CampusId { get; set; }

        [Display(Name = "Building")]
        public int? BuildingId { get; set; }

        [Display(Name = "Classroom")]
        public int? ClassroomId { get; set; }

        [Display(Name = "Part-of-Term")]
        public int? PartOfTermId { get; set; }

        [Display(Name = "Section Status")]
        public int? SectionStatusId { get; set; }

        [Display(Name = "Section Time")]
        public int? SectionTimeId { get; set; }

        [Display(Name = "Section Pay Models")]
        public List<SectionPay> SectionPays { get; set; } = new List<SectionPay>();

        public IEnumerable<CheckboxItem> DaysOfWeek { get; set; } = new List<CheckboxItem>
        {
            new CheckboxItem { Text = "Mon", Value = "M" },
            new CheckboxItem { Text = "Tues", Value = "T" },
            new CheckboxItem { Text = "Wed", Value = "W" },
            new CheckboxItem { Text = "Thur", Value = "H" },
            new CheckboxItem { Text = "Fri", Value = "F" },
        };

        public int MaxEnrollment { get; set; } = 30;

        public string? SectionNotes { get; set; }
    }
}
