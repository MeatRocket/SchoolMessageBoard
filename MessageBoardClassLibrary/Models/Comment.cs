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
        public string PostId { get; set; }
        public Post Post { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string Value { get; set; }
    }
}
