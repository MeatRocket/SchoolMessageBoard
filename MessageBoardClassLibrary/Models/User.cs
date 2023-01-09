﻿using System.Collections;
using System.ComponentModel.DataAnnotations;

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
        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

        public virtual ICollection<Template> TemplateDetails { get; set; } = new List<Template>();

        [Required]
        public virtual ICollection<SchoolUser> Schools { get; set; }

    }
}