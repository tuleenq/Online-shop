using MediatR;
using MyShop.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using MyShop.Infrastructure.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

public class CreateProductCommand : IRequest<ProductResponse>
{
   
   
    public string Name { get; set; }
    public string Description { get; set; }
    
    public decimal Amount { get; set; }
   
    public int StockQuantity { get; set; }
    
    
    public int CategoryId { get; set; }
   
    public IFormFile ImageFile { get; set; }
    public int BrandId { get; set; }

}
public class ProductResponse
{
    public Product Product { get; set; }
    public List<CategoryDto> Categories { get; set; }
}
public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
}




public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductResponse>
{
    private readonly ApplicationDbContext _context;

    public CreateProductCommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // Fetch the category by its ID
        var category = await _context.category
            .FirstOrDefaultAsync(c => c.Id == request.CategoryId, cancellationToken);

        if (category == null)
        {
            throw new KeyNotFoundException("Category not found.");
        }

        string imagePath = null;

        // Handle image upload if an image is provided
        if (request.ImageFile != null)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = $"{Guid.NewGuid()}_{request.ImageFile.FileName}";
            imagePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await request.ImageFile.CopyToAsync(fileStream);
            }

            // Save relative path to the database
            imagePath = Path.Combine("images", uniqueFileName);
        }

        // Create the product
        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Amount,
            StockQuantity = request.StockQuantity,
            CtegoryId = request.CategoryId,
            ImagePath = imagePath, // Save the image path
            BrandId = request.BrandId
        };

        // Add the product to the database
        _context.Products.Add(product);
        await _context.SaveChangesAsync(cancellationToken);

        // Return the ProductResponse
        return new ProductResponse
        {
            Product = product
        };
    }
}
