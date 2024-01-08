using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    public class AcademicProgram
    {
        [Key]
        public int Id { get; set; }

        #region Add Fields
        [Required]
        public string? ProgramName { get; set; }

        [Required]
        public string? ProgramCode { get; set; }
        #endregion
    }
}
