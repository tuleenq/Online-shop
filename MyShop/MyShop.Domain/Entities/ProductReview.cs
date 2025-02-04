using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Domain.Entities
{
     public class ProductReview
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public string ReviewText { get; set; }
        

        // Navigation Properties
        public Product Product { get; set; }
        public User User { get; set; }
    }
}
