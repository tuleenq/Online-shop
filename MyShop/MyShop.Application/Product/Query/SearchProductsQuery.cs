using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyShop.Infrastructure.Data;

public class ProductDTO
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public byte[] ImageData { get; set; }
}

public class SearchProductsQuery : IRequest<List<ProductDTO>>
{
    public string SearchTerm { get; set; }
}

public class SearchProductsQueryHandler : IRequestHandler<SearchProductsQuery, List<ProductDTO>>
{
    private readonly ApplicationDbContext _context;

    public SearchProductsQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProductDTO>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
    {
        // Fetch products from the database
        var products = await _context.Products
            .AsNoTracking()
            .Where(p => p.Name.Contains(request.SearchTerm) || p.Description.Contains(request.SearchTerm))
            .ToListAsync(cancellationToken);

        // Convert products to ProductDTO with image data
        var productDTOs = new List<ProductDTO>();

        foreach (var product in products)
        {
            byte[] imageData = null;

            // If the image path exists, load the image as a byte array
            if (!string.IsNullOrEmpty(product.ImagePath))
            {
                var fullImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", product.ImagePath);
                if (File.Exists(fullImagePath))
                {
                    imageData = await File.ReadAllBytesAsync(fullImagePath, cancellationToken);
                }
            }

            productDTOs.Add(new ProductDTO
            {
                Name = product.Name,
                Price = product.Price,
                ImageData = imageData
            });
        }

        return productDTOs;
    }
}
