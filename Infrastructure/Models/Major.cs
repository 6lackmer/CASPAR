using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Infrastructure.Models
{
    public class Major
    {
        [Key]
        public int Id { get; set; }

        #region Add Fields
        [Required]
        [Display(Name = "Major Name")]
        public string? MajorName { get; set; }
        #endregion
    }
}
