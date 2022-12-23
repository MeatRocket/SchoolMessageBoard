using Microsoft.AspNetCore.Mvc;

namespace AdminPortal.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
