using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CA_Project.Models
{
    public class Cart
    {
        public Cart()
        {
            Id = new Guid();
            CartProducts = new List<CartProduct>();
        }
        public Guid Id { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<CartProduct> CartProducts { get; set; }
    }
}
