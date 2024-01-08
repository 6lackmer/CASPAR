using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class SectionTime
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Credit Hours")]
        public int CreditHours { get; set; }

        [Required]
        [Display(Name = "Days Per Week")]
        public int DaysPerWeek { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        public TimeSpan StartTime { get; set; }

        [Required]
        [Display(Name = "End Time")]
        public TimeSpan EndTime { get; set; }

        [Required]
        [Display(Name = "Display Text")]
        public string DisplayText { get; set; } = string.Empty;

        public virtual ICollection<Section>? Sections { get; set; }
    }
}
