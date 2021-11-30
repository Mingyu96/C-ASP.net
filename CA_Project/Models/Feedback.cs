using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CA_Project.Models
{
    public class Feedback
    {
        public Feedback()
        {
            Id = new Guid();
        }

        public Guid Id { get; set; }
        [Required][MaxLength(200)]
        public string Details { get; set; }
        [Range(0,5)][Required]
        public int Rating { get; set; }

        public virtual Product Product { get; set; }
        public virtual User User { get; set; }
    }
}
