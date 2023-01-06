
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

            return View(Template, new PostViewModel());
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
            postViewModel.ErrorMessages.Clear();
            postViewModel.Template = templatePage;

            if (postViewModel.files != null)
            {
                postViewModel.Media = new List<Media>();
                if (postViewModel.files.FirstOrDefault(x => !x.ContentType.Split("/")[0].Equals("Image", StringComparison.OrdinalIgnoreCase) && !x.ContentType.Split("/")[0].Equals("Video", StringComparison.OrdinalIgnoreCase)) != null)
                {
                    postViewModel.ErrorMessages.Add("Please Upload Videos and Images Only");
                    return RedirectToAction(templatePage, postViewModel);
                }

                postViewModel.Media = GetMedia(postViewModel.files);
            }

            User User = _context.Users.FirstOrDefault(x => x.Id == _accessor.HttpContext.Session.GetString("UserSession"));

            User.Posts = new List<Post>
            {
                postViewModel.MapToPost()
            };

            _context.Update(User);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult PostTemplate1(PostViewModel postViewModel)
        {
            return View(postViewModel);
        }

        public IActionResult PostTemplate2(PostViewModel postViewModel)
        {
            return View(postViewModel);
        }

        public IActionResult PostTemplate3(PostViewModel postViewModel)
        {
            return View(postViewModel);
        }

        public IActionResult DynamicSubmitPostView()
        {
            return View();
        }

        public IActionResult DynamicSubmitPost(Template template)
        {
            template.Id = Guid.NewGuid().ToString();
            List<Template> Templates = _context.TemplateDetails.ToList();

            if (Templates.Count > 0)
            {
                if (Templates.FirstOrDefault(x => x.PopertyName == template.PopertyName) != null)
                {
                    ModelState.AddModelError("UniquePropNameError", "Property Name Must Be Unique For Each Template Name!");
                    return View("DynamicSubmitPostView");
                }
            }

            _context.TemplateDetails.Add(template);
            _context.SaveChanges();

            return View("DynamicSubmitPostView");
        }

        public IActionResult DynamicPostView(string templateName)
        {
            List <Template> Templates = _context.TemplateDetails.OrderBy(x => x.PopertySequence).ToList().Where(x => x.Name.Equals(templateName, StringComparison.OrdinalIgnoreCase)).ToList();

            return View(Templates);
        }

        public IActionResult DynamicTemplatePicker()
        {
            List<Template> Templates = _context.TemplateDetails.ToList().DistinctBy(x => x.Name).ToList();
            List<string> TemplateNames = Templates.Select(x => x.Name).ToList();

            return View(TemplateNames);
        }

        [HttpGet]
        public IActionResult DynamicTemplateEditor(string TemplateName)
        {
            List<Template> Templates = _context.TemplateDetails.Where(x => x.Name == TemplateName).ToList();

            return View(Templates);
        }

        [HttpPost]
        public async Task <IActionResult> DynamicTemplateEditor()
        {
            var x = HttpContext.Request.Body;
            var reader = new StreamReader(x);
            var body = reader.ReadToEndAsync();

            var y = body.Result.Substring(0, body.Result.IndexOf("&__RequestVerificationToken")).Split("&").Select(z => z.Replace("+", " ")).ToList();
            //if count of substring is less than the property count  throw error

            string TemplateName = y[0].Split("=")[1];

            for (int i=1; i<y.Count; i++)
            {
                string[] KeyValuePairs = y[i].Split("=");
                _context.TemplateDetails.Where(x => x.Name  == TemplateName).ToList().First(x => x.PopertyName == KeyValuePairs[0]).PopertyValue = KeyValuePairs[1];
            }
            _context.SaveChanges();
            //var y = HttpContext.Request.BodyReader;
            return RedirectToAction("DynamicPostView", new{ templateName = TemplateName });
        }


        public List<Media> GetMedia(ICollection<IFormFile> formFiles)
        {
            List<Media> mediaList = new List<Media>();
            PostViewModel postViewModel = new();
            string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadedFiles");
            string MediaId;

            foreach (IFormFile file in formFiles)
            {
                MediaId = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                Stream stream = new FileStream(Path.Combine(savePath, MediaId), FileMode.Create);
                file.CopyTo(stream);
                mediaList.Add(new() { Id = MediaId, Name = file.FileName, Type = file.ContentType });
            }

            return mediaList;
        }
    }
}
