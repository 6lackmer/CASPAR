using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class SectionPay
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Section")]
        public int SectionId { get; set; }

        [ForeignKey("SectionId")]
        public Section? Section { get; set; }

        [Display(Name = "Pay Model")]
        public int? PayModelId { get; set; }

        [ForeignKey("PayModelId")]
        public PayModel? PayModel { get; set; }

        [Display(Name = "Funding Source")]
        public int? PayOrgId { get; set; }

        [ForeignKey("PayOrgId")]
        public PayOrg? PayOrg { get; set; }

        public decimal CreditHours { get; set; }
    }
}
