using KataBlog.Models;
using X.PagedList;

namespace KataBlog.ViewModels
{
    public class HomeVM
    {
        public string? Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? ThumbnailUrl { get; set; }
        public IPagedList<Post>? Posts { get; set; }
    }
}
