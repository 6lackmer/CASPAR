using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    public class PayModel
    {
        [Key]
        public int Id { get; set; }

        #region Add Fields
        [Required]
        [Display(Name = "Pay Model Name")]
        public string? PayModelTitle { get; set; }

        public bool IsDisabled { get; set; } = false;
        #endregion
    }
}
