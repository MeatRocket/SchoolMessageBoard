
using AdminPortal.Models;
using MessageBoardClassLibrary.MessageBoardContext;
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
    }
}
