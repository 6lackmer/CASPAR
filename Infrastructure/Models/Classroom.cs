using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class Classroom
    {
        [Key]
        public int Id { get; set; }

        #region Add Foreign Key(s)
        [Required]
        [Display(Name = "Building")]
        public int BuildingId { get; set; }

        [ForeignKey("BuildingId")]
        public Building? Building { get; set; }

        public bool IsDisabled { get; set; } = false;
        #endregion

        #region Add Fields
        [Display(Name = "Details")]
        public string? ClassroomDetail { get; set; }

        [Display(Name = "Max Capacity")]
        public int? ClassroomCapacity { get; set; }

        [Required]
        [DisplayName("Class Number")]
        public string? ClassroomNumber { get; set; }

        public virtual ICollection<ClassroomFeature>? Features { get; set; }
        #endregion
    }
}
