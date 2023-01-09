
using AdminPortal.Models;
using MessageBoardClassLibrary.MessageBoardContext;
using MessageBoardClassLibrary.Migrations;
using MessageBoardClassLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using NuGet.Packaging;
using System.Collections.Generic;
using System.Security;
using System.Text;

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
            User user = _context.Users.First(x => x.Id == _accessor.HttpContext.Session.GetString("UserSession"));
            //_context.TemplateDetails.Add(template);
            user.TemplateDetails.Add(template);
            _context.Update(user);
            _context.SaveChanges();

            return View("DynamicSubmitPostView");
        }

        public IActionResult DynamicPostView(string templateName)
        {
            List<Template> Templates = _context.TemplateDetails.OrderBy(x => x.PopertySequence).ToList().Where(x => x.Name.Equals(templateName, StringComparison.OrdinalIgnoreCase)).ToList();

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
        public async Task<IActionResult> DynamicTemplateEditor(IFormCollection formColletion)
        {
            var changedTemplates = formColletion.ToList();
            string TemplateName = changedTemplates[0].Value;
            List<Template> TemplateComponents = _context.TemplateDetails.Where(x => x.Name == TemplateName).ToList();

            for (int i = 1; i < changedTemplates.Count - 1; i++)
            {
                var KeyValuePairs = changedTemplates[i];
                if (TemplateComponents.First(x => x.PopertyName == KeyValuePairs.Key) != null && TemplateComponents.First(x => x.PopertyName == KeyValuePairs.Key).PopertyType == "Media")
                {
                    TemplateComponents.First(x => x.PopertyName == KeyValuePairs.Key).PopertyValue = GetDynamicMediaFiles(KeyValuePairs.Value);
                }
                else
                    TemplateComponents.First(x => x.PopertyName == KeyValuePairs.Key).PopertyValue = KeyValuePairs.Value;
            }

            _context.Users.First(x => x.Id == _accessor.HttpContext.Session.GetString("UserSession")).TemplateDetails.AddRange(TemplateComponents);
            _context.SaveChanges();
            return RedirectToAction("DynamicPostView", new { templateName = TemplateName });
        }

        public string GetDynamicMediaFiles(Microsoft.Extensions.Primitives.StringValues files)
        {
            List<string> Files = new();
            string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/DynamicUploadedFiles");
            string MediaId;


            foreach (string fileName in files)
            {
                MediaId = Guid.NewGuid().ToString() + Path.GetExtension(fileName);
                Files.Add(MediaId);
                Stream stream = new FileStream(Path.Combine(savePath, MediaId), FileMode.Create);
                stream.CopyTo(stream);
            }

            return string.Join(" ", Files);
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
