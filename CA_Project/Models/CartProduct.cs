using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CA_Project.Models
{
    public class CartProduct
    {
        public CartProduct()
        {
            Id = new Guid();
        }
        public Guid Id { get; set; }
        [Required]
        public int Quantity { get; set; }
        public virtual Cart Cart { get; set; }
        public  virtual Product Product { get; set; }
    }
}
