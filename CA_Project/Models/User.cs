using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CA_Project.Models
{
    public class User
    {
        public User()
        {
            Id = new Guid();
            Feedbacks = new List<Feedback>();
            Purchases = new List<Purchase>();
        }
        public Guid Id { get; set; }
        [Required][MaxLength(20)]
        public string Username { get; set; }
        [Required][MaxLength(60)]
        public string Password { get; set; }
        [Required][MaxLength(50)]
        public string FullName { get; set; }
        [Required][MaxLength(50)]
        public string Email { get; set; }

        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<Purchase> Purchases { get; set; }
    }
}
