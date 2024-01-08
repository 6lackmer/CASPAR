using Infrastructure.Models;

namespace CASPAR.ViewModels
{
    public class InstructorSchedules
    {
        public int InstructorId { get; set; }

        public Instructor? Instructor { get; set; }

        public int? Load { get; set; }

        public int ReleaseTime { get; set; }

        public decimal Credits { get; set; }

        public IEnumerable<Section> Sections { get; set; } = new List<Section>();

        public IEnumerable<ReleaseTime> ReleaseTimes { get; set; } = new List<ReleaseTime>();
    }
}
