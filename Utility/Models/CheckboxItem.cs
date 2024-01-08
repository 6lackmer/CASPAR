using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Models
{
    public class CheckboxItem
    {
        public bool Checked { get; set; }

        public bool Disabled { get; set; }

        public string Text { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;
    }
}
