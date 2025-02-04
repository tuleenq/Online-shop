using MediatR;
using Microsoft.EntityFrameworkCore;
using MyShop.Domain.Entities;
using MyShop.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GetProductByNameQuery : IRequest<ProductDto>
{
    public string Name { get; set; }
}

public class GetProductByNameHandler : IRequestHandler<GetProductByNameQuery, ProductDto>
{
    private readonly ApplicationDbContext _context;

    public GetProductByNameHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProductDto> Handle(GetProductByNameQuery request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Name.ToLower() == request.Name.ToLower(), cancellationToken);

        if (product == null)
        {
            throw new KeyNotFoundException("Product not found.");
        }

        byte[] imageData = null;

        // If an image path exists, load the image as a byte array
        if (!string.IsNullOrEmpty(product.ImagePath))
        {
            var fullImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", product.ImagePath);
            if (File.Exists(fullImagePath))
            {
                imageData = await File.ReadAllBytesAsync(fullImagePath, cancellationToken);
            }
        }

        return new ProductDto
        {
           
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            ImageData = imageData
        };
    }
}

public class ProductDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public byte[] ImageData { get; set; }
}
