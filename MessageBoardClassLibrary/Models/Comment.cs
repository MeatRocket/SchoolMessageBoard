using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoardClassLibrary.Models
{
    public class Comment
    {
        public string Id { get; set; }
        public string CommentPostId { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string Value { get; set; }
        public DateTime DatePosted { get; set; }
    }
}
