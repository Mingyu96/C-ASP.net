using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CA_Project.Models
{
    public class Purchase
    {
        public Purchase()
        {
            Id = new Guid();
            PurchaseProducts = new List<PurchaseProduct>();
        }
        public Guid Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public float TotalAmount { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<PurchaseProduct> PurchaseProducts { get; set; }
    }
}
