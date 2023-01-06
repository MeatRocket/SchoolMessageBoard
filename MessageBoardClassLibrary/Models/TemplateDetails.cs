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
        public string ?Id { get; set; }

        [Required(ErrorMessage = "Please enter Template Name")]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter Property Name")]
        [MaxLength(200)]
        public string PopertyName { get; set; }

        [Required(ErrorMessage = "Please enter Property Type")]
        [MaxLength(200)]
        public string PopertyType { get; set; }

        [MaxLength(2000000)]
        public string ?PopertyValue { get; set; }

        [Required(ErrorMessage = "Please enter Property Sequence")]
        [MaxLength(200)]
        public string PopertySequence { get; set; }
    }
}
