using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoardClassLibrary.Models
{
    public class DbLog
    {
        public string Id { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string UserId { get; set; }
        public string TimeConsumed { get; set; }
        public DateTime Date { get; set; }
    }
}
