using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ProjectDataBaseCreation.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public RegisterModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        public void OnPost(string email)
        {
            Email = email;
        }
    }
}