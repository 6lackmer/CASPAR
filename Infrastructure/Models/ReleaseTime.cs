using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    // An instructor can have multiple release time objects per semester. 
    public class ReleaseTime
    {
        [Key]
        public int Id { get; set; }

        #region Add Foreign Key(s)
        [Required]
        [Display(Name = "Instructor")]
        public int InstructorId { get; set; }

        [ForeignKey("InstructorId")]
        public Instructor? Instructor { get; set; }

        [Required]
        [Display(Name = "Semester")]
        public int SemesterInstanceId { get; set; }

        [ForeignKey("SemesterInstanceId")]
        public SemesterInstance? SemesterInstance { get; set; }
                          
        //TODO: add a limit for the amount of credits for release times maybe 
        [Required]
        [Display(Name = "Semester Release Time")]
		public int ReleaseTimeAmount { get; set; }

        [Display(Name = "Notes")]
        public string? ReleaseTimeNotes { get; set; }
        #endregion
    }
}
