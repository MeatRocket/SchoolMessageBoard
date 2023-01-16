using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MessageBoardClassLibrary.Models
{
    public class Field
    {

        [Required]
        [MaxLength(200)]
        public string Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        [JsonIgnore]

        public virtual ICollection<Area> Areas { get; set; }

    }
}
