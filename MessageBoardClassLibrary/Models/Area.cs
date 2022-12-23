using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBoardClassLibrary.Models
{
    public class Area
    {

        [Required]
        [MaxLength(200)]
        public string Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        [MaxLength(200)]
        public string FieldId { get; set; }

        [Required]
        [MaxLength(200)]
        public Field Field { get; set; }
        public virtual ICollection<School> Schools { get; set; }

    }
}
