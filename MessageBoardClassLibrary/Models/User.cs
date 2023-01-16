using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MessageBoardClassLibrary.Models
{
    public class User
    {

        [Required]
        [MaxLength(200)]
        public string Id { get; set; }

        [Required]
        public bool IsValidated { get; set; } = false;

        [Required]
        public bool IsVisible { get; set; } = true;

        [Required]
        public bool IsActivated { get; set; } = false;

        [Required]
        [MaxLength(200)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(200)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(200)]
        public string Email { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Password { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Salt { get; set; }

        [Required]
        [MaxLength(100)]
        public string Role { get; set; }
        [Required]
        [JsonIgnore]
        public virtual ICollection<SchoolUser> Schools { get; set; }
        [JsonIgnore]

        public virtual ICollection<Post> Posts { get; set; }
        [JsonIgnore]

        public virtual ICollection<DynamicPost> DynamicPosts { get; set; }

    }
}