using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoardClassLibrary.Models
{
    public class SchoolUser
    {

        [Required]
        [MaxLength(200)]
        public string Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string UserId { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        [MaxLength(200)]
        public string SchoolId { get; set; }

        [Required]
        public School School { get; set; }
    }
}
