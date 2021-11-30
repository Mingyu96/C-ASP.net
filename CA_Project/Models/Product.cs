using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CA_Project.Models
{
    public class Product
    {
        public Product()
        {
            Id = new Guid();
            CartProducts = new List<CartProduct>();
            Feedbacks = new List<Feedback>();
            PurchaseProducts = new List<PurchaseProduct>();
        }
        public Guid Id { get; set; }
        [Required][MaxLength(50)]
        public string ProductName { get; set; }
        [Required]
        public float Price { get; set; }
        [Required][MaxLength(100)]
        public string Desc { get; set; }
        [Required][MaxLength(20)]
        public string ImageFile { get; set; }

       



        public virtual ICollection<CartProduct> CartProducts { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<PurchaseProduct> PurchaseProducts { get; set; }
    }
}
