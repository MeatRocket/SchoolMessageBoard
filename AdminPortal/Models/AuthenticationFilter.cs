using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Web.Mvc;

namespace AdminPortal.Models
{
    public class AuthenticationFilter : Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
        {
            if (context.HttpContext.Session.GetString("UserSession") == "" || context.HttpContext.Session.GetString("UserSession") == null || context.HttpContext.Session.GetString("UserRole") == "User")
            {
                context.Result = new HttpUnauthorizedResult();
            }
        }

    }
}
