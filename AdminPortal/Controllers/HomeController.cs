using AdminPortal.Models;
using MessageBoardClassLibrary.MessageBoardContext;
using MessageBoardClassLibrary.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography;
using static AdminPortal.Models.PasswordHasher;
using Microsoft.IdentityModel.Tokens;

namespace AdminPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private BoardContext _context { get; set; }
        private IHttpContextAccessor _accessor { get; set; }
        public HomeController(ILogger<HomeController> logger, BoardContext DbContext, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _context = DbContext;
            _accessor = httpContextAccessor;
        }

        public IActionResult Index(List<PostViewModel> RecentPosts)
        {
            if(RecentPosts == null || RecentPosts.Count == 0)
                RecentPosts = _context.Posts.Include(x => x.Media).OrderByDescending(x => x.DatePosted).Take(3).ToList().Select(x => x.MapToPostView()).ToList();

            if (RecentPosts.Count == 0)
                RecentPosts = null;

            return View(RecentPosts);
        }

        public IActionResult LogOut()
        {
            _accessor.HttpContext.Session.Clear();

            return RedirectToAction("SignIn", new UserViewModel());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult SignIn(UserViewModel userViewModel)
        {
            userViewModel.ErrorMessages.Clear();
            User user = _context.Users.FirstOrDefault(x => x.Email == userViewModel.Email);

            if (user == null)
            {
                userViewModel.ErrorMessages.Add("Use a Valid Email!");
                return View(userViewModel);
            }

            if (userViewModel.Password.IsNullOrEmpty())
            {
                userViewModel.ErrorMessages.Add("Enter A Passowrd!");
                return View(userViewModel);
            }
            var saltBytes = Convert.FromBase64String(user.Salt);
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(userViewModel.Password, saltBytes, 10000);

            if (Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256)) != user.Password)
            {
                userViewModel.ErrorMessages.Add("Incorrect Password!");
                return View(userViewModel);
            }

            if(!user.IsValidated || !user.IsActivated)
            {
                userViewModel.ErrorMessages.Add("Contact Admins To Activate or Validate your account");
                return View(userViewModel);
            }

            _accessor.HttpContext.Session.SetString("UserSession",user.Id);
            _accessor.HttpContext.Session.SetString("UserRole", user.Role);
            _accessor.HttpContext.Session.SetString("UserName", user.FirstName +" "+user.LastName);


            user.Password = null;

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult HomePage(string firstname)
        {
            return View(firstname);
        }

        public IActionResult Register(UserViewModel userViewModel, List<string> schools)
        {
            userViewModel.ErrorMessages.Clear();
            if (schools == null || schools.Count == 0)
                ModelState.AddModelError("schools", "Please Select A School");

            User user = new();
            userViewModel.Areas = _context.Areas.ToList();
            ViewBag.Fields = _context.Fields.ToList();
            userViewModel.Schools = _context.Schools.Include(x => x.Area.Field).ToList();

            if (ModelState.IsValid && userViewModel != null)
            {
                if (_context.Users.Where(x => x.Email == userViewModel.Email).FirstOrDefault() != null)
                {
                    userViewModel.ErrorMessages.Add("Email Already Exists Try a Different One");
                    return View(userViewModel);
                }

                List<SchoolUser> SchoolUsers = new();
                user = userViewModel.MapToUser();

                HashSalt hashsalt = PasswordHasher.GenerateSaltedHash(userViewModel.Password);
                user.Salt = hashsalt.Salt;
                user.Password = hashsalt.Hash;

                _context.Users.Add(user);

                foreach (string school_Id in schools)
                {
                    SchoolUsers.Add(new() { Id = Guid.NewGuid().ToString(), SchoolId = school_Id, UserId = user.Id });
                }

                _context.AddRange(SchoolUsers);

                _accessor.HttpContext.Session.SetString("UserSession", user.Id);
                _context.SaveChanges();

                return View("HomePage", user.MapToUserViewModel().FirstName);
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                var errors = ModelState.Values.ToList().Where(x => x.ValidationState == ModelValidationState.Invalid);
                foreach (var error in errors)
                {
                    userViewModel.ErrorMessages.Add(error.Errors[0].ErrorMessage);
                }
                return View(userViewModel);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}