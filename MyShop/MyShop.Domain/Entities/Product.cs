using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyShop.Domain.Entities
{
   
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsPaid { get; set; }
        public int CtegoryId { get; set; }
        public int? BrandId { get; set; }

        public string ImagePath { get; set; }
        [JsonIgnore]
        public Ctegory Ctegory { get; set; }
        public Brand? brand { get; set; }
       
        public ICollection<ProductReview> Reviews { get; set; }

    }
}
