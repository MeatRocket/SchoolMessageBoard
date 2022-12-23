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
using Microsoft.Data.SqlClient;

namespace AdminPortal.Controllers
{
    [AuthFilter]
    public class AdminController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private BoardContext _context { get; set; }
        private IHttpContextAccessor _accessor { get; set; }

        private int _pageSize = 10;

        public AdminViewModel Admin = new();
        public AdminController(ILogger<HomeController> logger, BoardContext DbContext, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _context = DbContext;
            _accessor = httpContextAccessor;
        }

        public IActionResult ManageUsers(string sortField, string sortOrder, string searchString, int PageNumber = 1)
        {
            Admin.Schools = _context.Schools.ToList();
            Admin.SchoolUsers = _context.SchoolUsers.ToList();
            Admin.Users = _context.Users.Include(x => x.Schools).Where(x => x.IsVisible == true).OrderBy(x => x.FirstName).ToList();
            ViewBag.TotalSize = (int)Math.Ceiling(Admin.Users.Count / (double)_pageSize);


            if (!searchString.IsNullOrEmpty())
            {
                string[] Name = searchString.Split(" ");
                if (Name.Length > 1)
                    Admin.Users = Admin.Users.Where(x => x.FirstName.Contains(Name[0], StringComparison.OrdinalIgnoreCase) && x.LastName.Contains(Name[1], StringComparison.OrdinalIgnoreCase)).ToList();
                else
                    Admin.Users = Admin.Users.Where(x => x.FirstName.Contains(Name[0], StringComparison.OrdinalIgnoreCase)).ToList();

                return View(Admin);
            }

            if (!Request.Query["PageNumber"].IsNullOrEmpty())
                ViewBag.PageNumber = int.Parse(Request.Query["PageNumber"]);

            Admin.Users = Admin.Users.Skip((PageNumber - 1) * _pageSize).Take(_pageSize).ToList();

            ViewBag.SortOrder = "";
            if (sortOrder.IsNullOrEmpty())
                ViewBag.SortOrder = "asc";

            switch (sortField)
            {
                case ("email"):
                    if (ViewBag.SortOrder == "asc")
                        Admin.Users = Admin.Users.OrderBy(x => x.Email).ToList();
                    else
                        Admin.Users = Admin.Users.OrderByDescending(x => x.Email).ToList();
                    break;

                case ("role"):
                    if (ViewBag.SortOrder == "asc")
                        Admin.Users = Admin.Users.OrderBy(x => x.Role).ToList();
                    else
                        Admin.Users = Admin.Users.OrderByDescending(x => x.Role).ToList();
                    break;


                case ("valid"):
                    if (ViewBag.SortOrder == "asc")
                        Admin.Users = Admin.Users.OrderBy(x => x.IsValidated).ToList();
                    else
                        Admin.Users = Admin.Users.OrderByDescending(x => x.IsValidated).ToList();
                    break;

                case ("active"):
                    if (ViewBag.SortOrder == "asc")
                        Admin.Users = Admin.Users.OrderBy(x => x.IsActivated).ToList();
                    else
                        Admin.Users = Admin.Users.OrderByDescending(x => x.IsActivated).ToList();
                    break;

                default:
                    if (!(ViewBag.SortOrder == "asc"))
                        Admin.Users = Admin.Users.OrderByDescending(x => x.FirstName).ToList();
                    else
                        Admin.Users = Admin.Users.OrderBy(x => x.FirstName).ToList();
                    break;
            }


            return View(Admin);
        }

        [HttpGet]
        public IActionResult EditUser(string Id)
        {
            Admin.EditUser = _context.Users.FirstOrDefault(x => x.Id == Id);
            Admin.Fields = _context.Fields.ToList();
            Admin.Schools = _context.Schools.Include(x => x.Area.Field).ToList();
            Admin.Areas = _context.Areas.ToList();

            if (Admin.EditUser == null)
            {
                Admin.EditSchool = _context.Schools.FirstOrDefault(x => x.Id == Id);
                return View("EditSchool", Admin);
            }

            return View("EditUser", Admin);
        }
        [HttpPost]
        public IActionResult EditUser(AdminViewModel adminViewModel, List<string> schools, string IsActivated, string IsValidated)
        {
            Admin.EditUser = _context.Users.FirstOrDefault(x => x.Id == adminViewModel.EditUser.Id);
            Admin.SchoolUsers = _context.SchoolUsers.Where(x => x.UserId == adminViewModel.EditUser.Id).ToList();

            if (adminViewModel.Email != Admin.EditUser.Email)
                Admin.EditUser.Email = adminViewModel.Email;

            if (adminViewModel.FirstName != Admin.EditUser.FirstName)
                Admin.EditUser.FirstName = adminViewModel.FirstName;

            if (adminViewModel.LastName != Admin.EditUser.LastName)
                Admin.EditUser.LastName = adminViewModel.LastName;

            if (adminViewModel.Role != null && adminViewModel.Role != Admin.EditUser.Role)
                Admin.EditUser.Role = adminViewModel.Role;

            if (!adminViewModel.Password.IsNullOrEmpty())
            {
                var saltBytes = Convert.FromBase64String(Admin.EditUser.Salt);
                var rfc2898DeriveBytes = new Rfc2898DeriveBytes(adminViewModel.Password, saltBytes, 10000);

                if (Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256)) != Admin.EditUser.Password)
                {
                    HashSalt hashsalt = PasswordHasher.GenerateSaltedHash(adminViewModel.Password);
                    Admin.EditUser.Password = hashsalt.Hash;
                    Admin.EditUser.Salt = hashsalt.Salt;
                }
            }

            if (IsValidated == "on" || IsValidated == "value")
            {
                if (!Admin.EditUser.IsValidated)
                    Admin.EditUser.IsValidated = true;
            }
            else
            {
                if (Admin.EditUser.IsValidated)
                    Admin.EditUser.IsValidated = false;
            }

            if (IsActivated == "on" || IsActivated == "value")
            {
                if (!Admin.EditUser.IsActivated)
                    Admin.EditUser.IsActivated = true;
            }
            else
            {
                if (Admin.EditUser.IsActivated)
                    Admin.EditUser.IsActivated = false;
            }

            if (!schools.IsNullOrEmpty())
            {
                _context.SchoolUsers.RemoveRange(Admin.SchoolUsers);
                Admin.SchoolUsers.Clear();

                foreach (string school_Id in schools)
                {
                    SchoolUser schoolUser = Admin.SchoolUsers.FirstOrDefault(x => x.SchoolId == school_Id && x.UserId == Admin.EditUser.Id);
                    if (schoolUser == null)
                    {
                        Admin.SchoolUsers.Add(new() { Id = Guid.NewGuid().ToString(), SchoolId = school_Id, UserId = Admin.EditUser.Id });
                    }
                }

                _context.SchoolUsers.AddRange(Admin.SchoolUsers);
            }

            _context.Update(Admin.EditUser);
            _context.SaveChanges();
            AdminViewModel admin = new();
            admin.Users = _context.Users.ToList();
            return RedirectToAction("ManageUsers");
        }
        [HttpGet]
        public IActionResult DeleteUser(string UserId)
        {
            Admin.EditUser = _context.Users.FirstOrDefault(x => x.Id == UserId);
            Admin.EditUser.IsVisible = false;
            _context.Update(Admin.EditUser);
            _context.SaveChanges();
            return Json(UserId);
        }
        [HttpGet]
        public IActionResult ManageSchools(string sortField, string sortOrder, string searchString)
        {
            if (!searchString.IsNullOrEmpty())
            {
                Admin.Schools = _context.Schools.Include(x => x.Area.Field).Where(x => x.Name.Contains(searchString)).OrderBy(x => x.Name).ToList();
                return View(Admin);
            }

            ViewBag.SortOrder = "";
            if (sortOrder.IsNullOrEmpty())
                ViewBag.SortOrder = "asc";

            Admin.Schools = _context.Schools.Include(x => x.Area.Field).ToList();

            switch (sortField)
            {
                case ("fieldName"):
                    if (ViewBag.SortOrder == "asc")
                        Admin.Schools = Admin.Schools.OrderBy(x => x.Area.Field.Name).ToList();
                    else
                        Admin.Schools = Admin.Schools.OrderByDescending(x => x.Area.Field.Name).ToList();
                    break;

                case ("areaName"):
                    if (ViewBag.SortOrder == "asc")
                        Admin.Schools = Admin.Schools.OrderBy(x => x.Area.Name).ToList();
                    else
                        Admin.Schools = Admin.Schools.OrderByDescending(x => x.Area.Name).ToList();
                    break;

                default:
                    if (ViewBag.SortOrder == "asc")
                        Admin.Schools = Admin.Schools.OrderBy(x => x.Name).ToList();
                    else
                        Admin.Schools = Admin.Schools.OrderByDescending(x => x.Name).ToList();
                    break;
            }

            return View(Admin);
        }

        [HttpPost]
        public IActionResult EditSchool(AdminViewModel adminViewModel)
        {
            Admin.EditSchool = _context.Schools.FirstOrDefault(x => x.Id == adminViewModel.EditSchool.Id);
            Admin.Areas = _context.Areas.ToList();

            if (adminViewModel.SchoolName != Admin.EditSchool.Name)
                Admin.EditSchool.Name = adminViewModel.SchoolName;

            if (adminViewModel.AreaId != Admin.EditSchool.Area.Id)
                Admin.EditSchool.Area = _context.Areas.FirstOrDefault(x => x.Id == adminViewModel.AreaId);

            _context.Update(Admin.EditSchool);
            _context.SaveChanges();

            return RedirectToAction("ManageSchools");
        }

        [HttpGet]
        public IActionResult EditSchool(string SchoolId)
        {
            Admin.EditSchool = _context.Schools.FirstOrDefault(x => x.Id == SchoolId);
            Admin.Areas = _context.Areas.ToList();

            return View("EditSchool", Admin);
        }
    }
}
