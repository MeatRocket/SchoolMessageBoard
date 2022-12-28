using MessageBoardClassLibrary.Models;
using Microsoft.CodeAnalysis.Differencing;

namespace AdminPortal.Models
{
    public class PostViewModel : Post
    {
        public Post EditPost { get; set; }
    }
}
