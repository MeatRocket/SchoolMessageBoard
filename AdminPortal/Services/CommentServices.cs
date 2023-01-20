using MessageBoardClassLibrary.MessageBoardContext;
using MessageBoardClassLibrary.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Packaging;
using System.Xml.Linq;

namespace AdminPortal.Services
{
    public delegate void MyEventHandler();

    public class CommentServices : Hub
    {
        public event MyEventHandler MyEventHandler;
        public BoardContext _dbcontext { get; set; }
        public List<Comment> Comments { get; set; } = new();
        public string CommentValue { get; set; }
        public string PostId { get; set; }
        public string UserId { get; set; }


        public async Task UpdateComments()
        {
            if (CommentValue.IsNullOrEmpty())
                return;

            MyEventHandler?.Invoke();
            _dbcontext.Comments.Add(new Comment() { Id = Guid.NewGuid().ToString(), CommentPostId = PostId, Value = CommentValue, UserId = UserId, DatePosted = DateTime.Now });
            _dbcontext.SaveChanges();
            Comments = _dbcontext.Comments.Include(x=> x.User).Where(x => x.CommentPostId == PostId).ToList().OrderByDescending(x => x.DatePosted).ToList();
            MyEventHandler?.Invoke();
        }

    }
}
