using MessageBoardClassLibrary.MessageBoardContext;
using MessageBoardClassLibrary.Models;
using System.Xml.Linq;

namespace AdminPortal.Services
{
    public delegate void MyEventHandler();

    public class CommentServices
    {
        public event MyEventHandler MyEventHandler;
        public BoardContext _dbcontext { get; set; }
        public List<Comment> Comments { get; set; }

        public void UpdateComments(string CommentValue,string PostId, string UserId)
        {
            _dbcontext.Posts.FirstOrDefault(x => x.Id == PostId).Comments.Add(new Comment() { Id = Guid.NewGuid().ToString(), PostId = PostId, Value = CommentValue, UserId = UserId});
            _dbcontext.SaveChanges();
            MyEventHandler?.Invoke();
            Comments = _dbcontext.Comments.Where(x => x.PostId == PostId).ToList();
        }
    }
}
