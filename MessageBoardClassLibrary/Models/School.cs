using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MessageBoardClassLibrary.Models
{
    public class School
    {

        [Required]
        [MaxLength(200)]
        public string Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        [JsonIgnore]

        public virtual ICollection<User> Users { get; set; }

        [Required]
        [MaxLength(200)]
        public string AreaId { get; set; }

        [Required]
        public Area Area { get; set; }
    }
}
