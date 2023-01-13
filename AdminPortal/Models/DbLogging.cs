using MessageBoardClassLibrary.MessageBoardContext;
using MessageBoardClassLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

namespace AdminPortal.Models
{
    public class DbLogger 
    {
        private BoardContext _dbcontext { get; set; }
        public DbLogger(BoardContext BoardContext)
        {
            _dbcontext = BoardContext;
        }

        public void DbLog(ActionContext actionContext, long TimeElapsed)
        {
            string uid  = actionContext.HttpContext.Session.GetString("UserSession");
            if (uid.IsNullOrEmpty())
                uid = "";

            DbLog dbLog = new()
            {
                Id = Guid.NewGuid().ToString(),
                ActionName = actionContext.ActionDescriptor.DisplayName,
                UserId = uid,
                Date = DateTime.Now,
                ControllerName = actionContext.RouteData.Values["controller"].ToString(),
                TimeConsumed = TimeElapsed.ToString()
            };

            _dbcontext.Add(dbLog);
            _dbcontext.SaveChanges();
        }

    }

    public class DbLogging : Attribute, IActionFilter
    {
        public DbLogger _logger;
        private Stopwatch stopwatch;

        public DbLogging(DbLogger logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            stopwatch = Stopwatch.StartNew();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            stopwatch.Stop();
            context.RouteData.Values.Add("controller", context.Controller.GetType().ToString());
            _logger.DbLog(context, stopwatch.ElapsedMilliseconds);
        }

    }
}
