
using AdminPortal.Models;
using MessageBoardClassLibrary.MessageBoardContext;
using MessageBoardClassLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;

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

        public IActionResult PostPage(string Template)
        
        {
            if (Template.IsNullOrEmpty())
                return View();

            return View(Template);
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 500000000)]
        [RequestSizeLimit(500000000)]
        public IActionResult SubmitPost(PostViewModel postViewModel, string templatePage)
        {
            if (postViewModel == null)
            {
                postViewModel = new();
                postViewModel.ErrorMessages.Add("File limit should not exceed 500MB!");
                return RedirectToAction(templatePage, postViewModel);
            }

            postViewModel.DatePosted = DateTime.Now;
            postViewModel.Media = new List<Media>();
            postViewModel.ErrorMessages.Clear();

            if (postViewModel.files.FirstOrDefault(x => x.ContentType.Split("/")[0].Equals("Image", StringComparison.OrdinalIgnoreCase) || x.ContentType.Split("/")[0].Equals("Video", StringComparison.OrdinalIgnoreCase)) == null)
            {
                postViewModel.ErrorMessages.Add("Please Upload Videos and Images Only");
                return RedirectToAction(templatePage, postViewModel);
            }

            postViewModel.Media = GetMedia(postViewModel.files);

            User User = _context.Users.FirstOrDefault(x => x.Id == _accessor.HttpContext.Session.GetString("UserSession"));
            User.Posts = new List<Post>
            {
                postViewModel.MapToPost()
            };

            _context.Update(User);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public List<Media> GetMedia(ICollection<IFormFile> formFiles)
        {
            List<Media> mediaList = new List<Media>();
            PostViewModel postViewModel = new();
            string savePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");
            string MediaId;

            foreach (IFormFile file in formFiles)
            {
                MediaId = Guid.NewGuid().ToString();
                Stream stream = new FileStream(Path.Combine(savePath, MediaId + Path.GetExtension(file.FileName)), FileMode.Create);
                file.CopyTo(stream);
                mediaList.Add(new() { Id = MediaId, Name = file.FileName, Type = file.ContentType });
            }

            return mediaList;
        }
    }
}
