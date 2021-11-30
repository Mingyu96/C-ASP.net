using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CA_Project.Models
{
    public class Session
    {
        public Session()
        {
            Id = new Guid();
            TimeStamp = DateTimeOffset.Now.ToUnixTimeSeconds();
        }

        public Guid Id { get; set; }
        [Required]
        public long TimeStamp { get; set; }

        public virtual User User { get; set; }

        public const int timeout = 30*60;
    }
}
