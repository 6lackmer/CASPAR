using Infrastructure.Models;

namespace CASPAR.ViewModels
{
    public class ScheduleSection
    {
        public Section Section { get; set; } = new();

        public bool Assigned { get; set; }
    }
}
