using Infrastructure.Models;

namespace CASPAR.ViewModels
{
    public class WishlistSummary
    {
        public int Id { get; set; }

        public Modality? Modality { get; set; }

        public Campus? Campus { get; set; }

        public TimeBlock? TimeBlock { get; set; }

        public int Count { get; set; }
    }
}
