namespace ShopRazor.models
{
    public class Poducts
    {
       
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public int StockQuantity { get; set; }
            public bool IsPaid { get; set; }
       
    }
}
