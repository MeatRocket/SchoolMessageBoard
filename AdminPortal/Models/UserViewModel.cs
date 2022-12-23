using MessageBoardClassLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AdminPortal.Models
{
    public class UserViewModel
    {
        public List<string> ErrorMessages { get; set; } = new();
        [Required]
        [MaxLength(200)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(200)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [MaxLength(200)]
        public string? Email { get; set; }

        [Required]
        [MaxLength(200)]
        [Compare("Email", ErrorMessage = "Email doesn't match")]
        [Display(Name = "Confirm Email")]

        public string? ConfirmEmail { get; set; }

        [Required]
        [MaxLength(200)]
        public string? Password { get; set; }

        [Required]
        [MaxLength(200)]
        [Compare("Password", ErrorMessage = "Password doesnt match!")]
        [Display(Name = "Confirm Password")]
        public string? ConfirmPassword { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Role { get; set; }

        public ICollection<School>? Schools { get; set; }
        public ICollection<Area>? Areas { get; set; }

    }
}
