using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace AdminPortal.Models;

public class AdminAuthFilter : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.HttpContext.Session.GetString("UserSession").IsNullOrEmpty() || context.HttpContext.Session.GetString("UserRole") != "Admin")
            context.Result = new StatusCodeResult(StatusCodes.Status401Unauthorized);
    }
}