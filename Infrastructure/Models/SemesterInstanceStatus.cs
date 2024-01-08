using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class SemesterInstanceStatus
    {
        [Key]
        public int Id { get; set; }

        #region Add Fields
        [Required]
        public string? Status { get; set; }

        public string Description { get; set; } = string.Empty;
        #endregion
    }
}
