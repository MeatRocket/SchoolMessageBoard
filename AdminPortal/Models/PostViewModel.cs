using MessageBoardClassLibrary.Models;
using Microsoft.CodeAnalysis.Differencing;

namespace AdminPortal.Models
{
    public class PostViewModel : Post
    {
        public Post EditPost { get; set; }
        public List<string> ErrorMessages { get; set; } = new();
        public ICollection<IFormFile> files { get; set; }
    }
}
