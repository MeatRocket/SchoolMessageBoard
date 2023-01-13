using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoardClassLibrary.Models
{
    public class DynamicProperty
    {
        [MaxLength(200)]
        public string? Id { get; set; }

        [Required(ErrorMessage = "Please enter Property Name")]
        [MaxLength(200)]
        public string PropertyName { get; set; }

        [Required(ErrorMessage = "Please enter Property Type")]
        [MaxLength(200)]
        public string Type { get; set; }

        [MaxLength(2000000)]
        public string? Value { get; set; }

        [Required(ErrorMessage = "Please enter Property Sequence")]
        [MaxLength(200)]
        public string Sequence { get; set; }
        public string ?TemplateId { get; set; }
        public Template ?Template { get; set; }
    }
}
