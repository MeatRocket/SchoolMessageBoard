using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoardClassLibrary.Models
{
    public class Post
    {

        [Required]
        [MaxLength(200)]
        public string Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string UserId { get; set; }

        [Required]
        [MaxLength(200)]
        public bool IsValid { get; set; } = false;

        [Required]
        public bool IsVisible { get; set; } = true;

        [Required]
        public DateTime DatePosted { get; set; }

        [Required]
        public User User { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public virtual ICollection<Media> Media { get; set; }

    }
}
