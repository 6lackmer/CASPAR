using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    public class PayOrg
    {
        [Key]
        public int Id { get; set; }

        #region  Add Fields
        [Required]
        [Display(Name = "Funding Source Name")]
        public string? PayOrgTitle { get; set; }

        public bool IsDisabled { get; set; } = false;
        #endregion
    }
}
