using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    public class Semester
    {
        [Key]
        public int Id { get; set; }

        #region Add Fields
        [Required]
        [Display(Name = "Semester Type")]
        public string? SemesterName { get; set; }

        [Display(Name = "Default Load Hours")]
        public int DefaultLoad { get; set; } = 12;

        public bool IsDisabled { get; set; } = false;
        #endregion
    }
}
