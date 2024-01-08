using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    public class SectionStatus
    {
        [Key]
        public int Id { get; set; }

        #region Add Fields
        [Required]
        [Display(Name = "Status Name")]
        public string? SectionStatusName { get; set; }

        [Required]
        [Display(Name = "Assigned Color")]
        public string? SectionStatusColor { get; set; }

        public bool IsDisabled { get; set; } = false;
        #endregion
    }
}
