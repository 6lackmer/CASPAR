using Infrastructure.Models;

namespace CASPAR.ViewModels
{
    public class ScheduleCourse
    {
        public Course Course { get; set; } = new();

        public int SectionCount { get; set; }

        public int UnassignedSectionCount { get; set; }
    }
}
