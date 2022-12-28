
using AdminPortal.Models;
using MessageBoardClassLibrary.MessageBoardContext;
using MessageBoardClassLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace AdminPortal.Controllers
{
    public class PostController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private BoardContext _context { get; set; }
        private IHttpContextAccessor _accessor { get; set; }

        public AdminViewModel Admin = new();
        public PostController(ILogger<HomeController> logger, BoardContext DbContext, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _context = DbContext;
            _accessor = httpContextAccessor;
        }

        public IActionResult PostPage()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SubmitPost(ICollection<IFormFile> files, PostViewModel Post)
        {
            Post.DatePosted = DateTime.Now;
            Post.Media = new List<Media>();

            string savePath = Path.Combine(Directory.GetCurrentDirectory(),"UploadedImages");
            string MediaId;
            foreach(IFormFile file in files)
            {
                MediaId = Guid.NewGuid().ToString();
                Stream stream = new FileStream(Path.Combine(savePath, MediaId + Path.GetExtension(file.FileName)), FileMode.Create);
                file.CopyTo(stream);
                Post.Media.Add(new() { Id = MediaId, Name = file.FileName, Type = file.ContentType});
            }

            return View("PostPage");
        }
    }
}
