using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CA_Project.Models
{
    public class AddToCartItem
    {
        public AddToCartItem()
        {
            Id = new Guid();
           
        }

        public Guid Id { get; set; }

       public int num { get; set; }



    }
}
