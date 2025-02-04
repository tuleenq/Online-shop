using MediatR;
using Microsoft.EntityFrameworkCore;
using MyShop.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GetProductsByCategoryAndBrandQuery : IRequest<List<ProducctDTO>>
{
    public string CategoryName { get; set; }
    public string BrandName { get; set; }
}
public class GetProductsByCategoryAndBrandQueryHandler : IRequestHandler<GetProductsByCategoryAndBrandQuery, List<ProducctDTO>>
{
    private readonly ApplicationDbContext _context;

    public GetProductsByCategoryAndBrandQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProducctDTO>> Handle(GetProductsByCategoryAndBrandQuery request, CancellationToken cancellationToken)
    {
        var products = await _context.Products
            .Include(p => p.Ctegory)
            .Include(p => p.brand)
            .Where(p => p.Ctegory.Name == request.CategoryName &&
                        (p.brand != null && p.brand.Name == request.BrandName))
            .ToListAsync(cancellationToken);

        var productDTOs = new List<ProducctDTO>();

        foreach (var product in products)
        {
            byte[] imageData = null;

            // Load the image as a byte array if the path exists
            if (!string.IsNullOrEmpty(product.ImagePath))
            {
                var fullImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", product.ImagePath);
                if (File.Exists(fullImagePath))
                {
                    imageData = await File.ReadAllBytesAsync(fullImagePath, cancellationToken);
                }
            }

            productDTOs.Add(new ProducctDTO
            {
                Name = product.Name,
                Price = product.Price,
                ImageData = imageData
            });
        }

        return productDTOs;
    }
}

public class ProducctDTO
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public byte[] ImageData { get; set; }
}

