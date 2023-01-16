using MessageBoardClassLibrary.MessageBoardContext;
using MessageBoardClassLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;

namespace AdminPortal.Models
{
    public class DbLogger : Attribute, IActionFilter
    {
        public BoardContext _context;
        public string name;
        private Stopwatch stopwatch = new();

        //public DbLogger(BoardContext _context)
        //{
        //    _context = _context;
        //}

        public void OnActionExecuted(ActionExecutedContext context)
        {
            stopwatch = Stopwatch.StartNew();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            stopwatch.Stop();

            string uid = context.HttpContext.Session.GetString("UserSession");

            if (uid.IsNullOrEmpty())
                uid = "";

            _context = context.HttpContext.RequestServices.GetRequiredService<BoardContext>();

            DbLog dbLog = new()
            {
                Id = Guid.NewGuid().ToString(),
                ActionName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName,
                UserId = uid,
                Date = DateTime.Now,
                ControllerName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ControllerName,
                TimeConsumed = stopwatch.ElapsedMilliseconds.ToString()
            };

            _context.Add(dbLog);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }


    public class DbLogging : IActionFilter, IDisposable, IFilterMetadata
    {
        public BoardContext _context;
        private Stopwatch stopwatch = new();


        public void OnActionExecuted(ActionExecutedContext context)
        {
            stopwatch = Stopwatch.StartNew();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            stopwatch.Stop();

            string uid = context.HttpContext.Session.GetString("UserSession");

            if (uid.IsNullOrEmpty())
                uid = "";

            _context = context.HttpContext.RequestServices.GetRequiredService<BoardContext>();

            DbLog dbLog = new()
            {
                Id = Guid.NewGuid().ToString(),
                ActionName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ActionName,
                UserId = uid,
                Date = DateTime.Now,
                ControllerName = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ControllerName,
                TimeConsumed = stopwatch.ElapsedMilliseconds.ToString()
            };

            _context.Add(dbLog);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

    }
}
