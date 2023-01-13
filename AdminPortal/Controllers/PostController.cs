
using AdminPortal.Models;
using MessageBoardClassLibrary.MessageBoardContext;
using MessageBoardClassLibrary.Migrations;
using MessageBoardClassLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using NuGet.Packaging;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace AdminPortal.Controllers
{
    [DbLogging]
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

            User User = _context.Users.First(x => x.Id == _accessor.HttpContext.Session.GetString("UserSession"));

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

        public IActionResult DynamicSubmitTemplateView()
        {
            return View();
        }

        public IActionResult DynamicSubmitTemplate(DynamicPostViewModel templateView)
        {
            templateView.Template.Id = Guid.NewGuid().ToString();
            Template Template = _context.DynamicTemplates.Include(x => x.DynamicProperties).FirstOrDefault(x => x.TemplateName == templateView.Template.TemplateName);

            if (Template == null)
            {
                //add Template if it doesnt exist
                templateView.Template.DynamicProperties.Add(new() { Id = Guid.NewGuid().ToString(), PropertyName = templateView.DynamicMedia.PropertyName, Sequence = templateView.DynamicMedia.Sequence, Type = templateView.DynamicMedia.Type });
                _context.DynamicTemplates.Add(templateView.Template);
                _context.SaveChanges();
                return View("DynamicSubmitTemplateView");

            }

            if (Template.DynamicProperties.FirstOrDefault(x => x.PropertyName == templateView.DynamicMedia.PropertyName) != null)
            {
                ModelState.AddModelError("UniquePropNameError", "Property Name Must Be Unique For Each Template Name!");
            }
            else
            {
                Template.DynamicProperties.Add(new() { Id = Guid.NewGuid().ToString(), PropertyName = templateView.DynamicMedia.PropertyName, Sequence = templateView.DynamicMedia.Sequence, Type = templateView.DynamicMedia.Type });
                _context.Update(Template);
                _context.SaveChanges();
            }
            return View("DynamicSubmitTemplateView");
        }

        public IActionResult DynamicPostView(string postId)
        {
            DynamicPost post = _context.DynamicPosts.Include(x => x.DynamicProperties.OrderBy(x => x.Sequence)).First(x => x.Id == postId);

            return View(post);
        }

        public IActionResult DynamicTemplatePicker()
        {
            List<Template> Templates = _context.DynamicTemplates.ToList();
            List<string> TemplateNames = Templates.Select(x => x.TemplateName).ToList();

            return View(TemplateNames);
        }

        [HttpGet]
        public IActionResult DynamicPostEditor(string templateName)
        {
            Template template = _context.DynamicTemplates.Include(x => x.DynamicProperties).First(x => x.TemplateName == templateName);
            template.DynamicProperties = template.DynamicProperties.OrderBy(x => x.Sequence).ToList();
            return View(template);
        }

        [HttpPost]
        public async Task<IActionResult> DynamicPostEditor(IFormCollection formColletion)
        {

            List<KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues>> changedTemplates = formColletion.ToList();
            string TemplateName = changedTemplates[0].Value;
            Template Template = _context.DynamicTemplates.Include(x => x.DynamicProperties).First(x => x.TemplateName == TemplateName);

            List<DynamicProperty> properties = Template.DynamicProperties;
            List<DynamicProperty> postProperties = new();

            foreach(DynamicProperty property in properties)
            {
                postProperties.Add(new() { Id = Guid.NewGuid().ToString(), PropertyName = property.PropertyName, Sequence = property.Sequence, Type = property.Type});
            }

            for (int i = 1; i < changedTemplates.Count - 1; i++)
            {
                KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues> KeyValuePairs = changedTemplates[i];
                postProperties.First(x => x.PropertyName == KeyValuePairs.Key).Value = KeyValuePairs.Value;
            }

            foreach (DynamicProperty property in postProperties.Where(x => x.Type == "Media"))
            {
                property.Value = GetDynamicMediaFile(formColletion.Files.Where(x => x.Name == property.PropertyName).ToList());
            }

            User user = _context.Users.First(x => x.Id == _accessor.HttpContext.Session.GetString("UserSession"));

            if (user.DynamicPosts == null || user.DynamicPosts.Count == 0)
                user.DynamicPosts = new List<DynamicPost>();

            DynamicPost dynamicPost = new (){ Id = Guid.NewGuid().ToString(), DynamicProperties = postProperties, DatePosted = DateTime.Now };

            _context.Users.First(x => x.Id == _accessor.HttpContext.Session.GetString("UserSession")).DynamicPosts.Add(dynamicPost);

            _context.SaveChanges();
            return RedirectToAction("DynamicPostView", new { postId = dynamicPost.Id});
        }

        public string GetDynamicMediaFile(List<IFormFile> files)
        {
            string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/DynamicUploadedFiles");
            string MediaId;
            StringBuilder SavedFiles = new StringBuilder();
            foreach (var file in files)
            {
                MediaId = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                Stream stream = new FileStream(Path.Combine(savePath, MediaId), FileMode.Create);
                file.CopyTo(stream);
                SavedFiles.Append(MediaId + " ");
            }

            return SavedFiles.ToString().Trim();
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
