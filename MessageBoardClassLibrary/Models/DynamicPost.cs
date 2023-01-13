using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoardClassLibrary.Models
{
    public class DynamicPost
    {
        [MaxLength(200)]
        public string? Id { get; set; }

        [MaxLength(200)]
        public string UserId { get; set; }
        public User User { get; set; }
        public List<DynamicProperty> DynamicProperties { get; set; } = new();
        public DateTime DatePosted { get; set; } = DateTime.Now;

    }
}
