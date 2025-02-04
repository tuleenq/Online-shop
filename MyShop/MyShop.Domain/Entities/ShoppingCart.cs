using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Domain.Entities
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? ProductName { get; set; }
        public int ProductId {  get; set; }
        public int Quantity { get; set; }
        public Product Product { get; set; }
    }
}
