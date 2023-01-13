using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoardClassLibrary.Models
{
    public class Template
    {
        [MaxLength(200)]
        public string? Id { get; set; }

        [Required(ErrorMessage = "Please enter Template Name")]
        [MaxLength(200)]
        public string TemplateName { get; set; }
        public List<DynamicProperty> DynamicProperties { get; set; } = new();
    }
}
