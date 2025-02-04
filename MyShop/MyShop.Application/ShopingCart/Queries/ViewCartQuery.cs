using MediatR;
using Microsoft.EntityFrameworkCore;
using MyShop.Domain.Entities;
using MyShop.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class ViewCartQuery : IRequest<IEnumerable<ShoppingDTO>>
{
    public int UserId { get; set; }
}
public class ViewCartHandler : IRequestHandler<ViewCartQuery, IEnumerable<ShoppingDTO>>
{
    private readonly ApplicationDbContext _context;

    public ViewCartHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ShoppingDTO>> Handle(ViewCartQuery request, CancellationToken cancellationToken)
    {
        var cartItems = await _context.ShoppingCarts
           .Include(sc => sc.Product)
           .AsNoTracking()
           .Where(sc => sc.UserId == request.UserId)
           .ToListAsync(cancellationToken);

        if (!cartItems.Any())
        {
            throw new KeyNotFoundException("No items found in the shopping cart.");
        }

        // Map to ShoppingDTO and handle image loading
        var shoppingDTOs = new List<ShoppingDTO>();
        foreach (var item in cartItems)
        {
            byte[] imageData = null;

            if (!string.IsNullOrEmpty(item.Product.ImagePath))
            {
                var fullImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", item.Product.ImagePath);
                if (File.Exists(fullImagePath))
                {
                    imageData = await File.ReadAllBytesAsync(fullImagePath, cancellationToken);
                }
            }

            shoppingDTOs.Add(new ShoppingDTO
            {
                name = item.Product.Name,
                price = (int)item.Product.Price, // Assuming you need price as an integer
                quantity = item.Quantity,
                ImageData = imageData
            });
        }

        return shoppingDTOs;
    
     }
}
public class ShoppingDTO
{
  public string name { get; set; }
  public int price { get; set; }
  public int quantity { get; set; }
  public byte[] ImageData { get; set; }

}