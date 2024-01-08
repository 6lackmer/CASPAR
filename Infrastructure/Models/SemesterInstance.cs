using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    public class SemesterInstance
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        #region Add Foreign Key(s)
        [Required]
        [Display(Name = "Semester Type")]
        public int SemesterId { get; set; }

        [ForeignKey("SemesterId")]
        public Semester? Semester { get; set; }

        [Required]
        [Display(Name = "Semester Instance Status")]
        public int SemesterInstanceStatusId { get; set; }

        [ForeignKey("SemesterInstanceStatusId")]
        public SemesterInstanceStatus? SemesterInstanceStatus { get; set; }
        #endregion

        #region Add Fields
        [Required]
        [Display(Name = "Registration Start Date")]
        [DisplayFormat(DataFormatString = "{0:D}")]
        public DateTime? SemesterRegistrationStart { get; set; }

        [Required]
        [Display(Name = "Registration End Date")]
        [DisplayFormat(DataFormatString = "{0:D}")]
        public DateTime? SemesterRegistrationEnd { get; set; }

        [Required]
        [Display(Name = "Semester Start Date")]
        [DisplayFormat(DataFormatString = "{0:D}")]
        public DateTime? SemesterStartDate { get; set; }

        [Required]
        [Display(Name = "Semester End Date")]
        [DisplayFormat(DataFormatString = "{0:D}")]
        public DateTime? SemesterEndDate { get; set; }

        [NotMapped]
        [Display(Name = "Scheduled")]
        public bool Scheduled { get; set; }
        #endregion
    }
}
