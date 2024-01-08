using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Models;

namespace Utility
{
    public class Functions
    {
        public static IEnumerable<CheckboxItem> ParseDaysOfWeek(string daysOfWeek)
        {
            IEnumerable<CheckboxItem> checkboxes = new List<CheckboxItem>
            {
                new CheckboxItem { Text = "Mon", Value = "M" },
                new CheckboxItem { Text = "Tues", Value = "T" },
                new CheckboxItem { Text = "Wed", Value = "W" },
                new CheckboxItem { Text = "Thur", Value = "H" },
                new CheckboxItem { Text = "Fri", Value = "F" },
            };
            foreach (var checkbox in checkboxes)
            {
                if (daysOfWeek.Contains(checkbox.Value))
                {
                    checkbox.Checked = true;
                }
            }
            return checkboxes;
        }
        public static string DaysOfWeekToString(IEnumerable<CheckboxItem> checkboxes)
        {
            return string.Join(null, checkboxes.Where(t => t.Checked).Select(t => t.Value));
        }
    }
}
