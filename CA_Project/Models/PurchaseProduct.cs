using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CA_Project.Models
{
    public class PurchaseProduct
    {
        public PurchaseProduct()
        {
            Id = new Guid();
        }
        public Guid Id { get; set; }
        [Required]
        public int ProductQuantity { get; set; }

        public virtual Product Product { get; set; }
        public virtual Purchase Purchase { get; set; }
    }
}
