using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    public class Building
    {
        [Key]
        public int Id { get; set; }

        #region Add Fk 
        [Required]
        [Display(Name = "Campus")]
        public int CampusId { get; set; }

        [ForeignKey("CampusId")]
        public Campus? Campus { get; set; }

        public bool IsDisabled { get; set; } = false;
        #endregion

        #region Add Fields
        [Required]
        [Display(Name = "Building Name")]
        public string? Name { get; set; }
        #endregion
    }
}
