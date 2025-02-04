using Microsoft.AspNetCore.Mvc;
using MyShop.Controllers;
using MyShop.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using MyShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace MyShop.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ApiControllercs
    {

        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("testdata")]
        public async Task<ActionResult> testData([FromQuery] CreateProductCommand command)
        {
            var Command = Mediator.Send(command);
            return Ok(Command);
        }

        [HttpGet("viewProducts")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await Mediator.Send(new ViewProductCommand());
            return Ok(products);
        }
        [HttpPost("GetProductById")]
        public async Task<IActionResult> GetProductById([FromBody] GetProductByIdRequest request)
        {

            var product = await Mediator.Send(new GetProductByIdQuery(request.Id));

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [HttpPost("GetProductByName")]
        public async Task<IActionResult> GetProductByName([FromBody] GetProductByNameQuery query)
        {
            if (query == null || string.IsNullOrWhiteSpace(query.Name))
                return BadRequest("Invalid product name.");

            var product = await Mediator.Send(query);

            if (product == null)
                return NotFound("Product not found.");

            return Ok(product);
        }
        //[Authorize(Roles = "Admin")]
        [HttpPost("createProduct")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ProductResponse>> CreateProduct([FromForm] CreateProductCommand request)
        {
            var productResponse = await Mediator.Send(request);
            return Ok(productResponse);
        }


        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductCommand command)
        {
            if (command == null)
                return BadRequest("Invalid product data.");

            var updatedProduct = await Mediator.Send(command);

            if (updatedProduct == null)
                return NotFound("Product not found.");

            return Ok(updatedProduct);
        }
       
        [HttpDelete("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct([FromBody] DeleteProductCommand command)
        {
            if (command == null || command.Id <= 0)
                return BadRequest("Invalid product ID.");

            var result = await Mediator.Send(command);

            if (!result)
                return NotFound("Product not found.");

            return Ok("Product deleted successfully.");
        }
        [HttpGet("search")]
        public async Task<ActionResult<List<Product>>> SearchProducts([FromQuery] string searchTerm)
        {
            var query = new SearchProductsQuery { SearchTerm = searchTerm };
            var products = await Mediator.Send(query);

            return Ok(products);
        }

        [HttpGet("by-category/{categoryName}")]
        public async Task<IActionResult> GetProductsByCategoryName(string categoryName)
        {
            var products = await _context.Products
                .Include(p => p.Ctegory) // Include category for filtering
                .Where(p => p.Ctegory.Name == categoryName)
                .ToListAsync();

            if (!products.Any())
                return NotFound($"No products found for category: {categoryName}");
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            // Map to ProductDto
            var productDtos = products.Select(p => new ProductDto
            {
                Name = p.Name,
                Price = p.Price,
                ImageFile = p.ImagePath != null ? $"{baseUrl}/{p.ImagePath}" : null // Map as needed; if you have a URL for the image, use that instead
            }).ToList();

            return Ok(productDtos);
        }

        [HttpGet("by-category-and-brand")]
        public async Task<ActionResult<List<ProductDTO>>> GetProductsByCategoryAndBrand([FromQuery] string categoryName, [FromQuery] string brandName)
        {
            var query = new GetProductsByCategoryAndBrandQuery
            {
                CategoryName = categoryName,
                BrandName = brandName
            };

            var products = await Mediator.Send(query);

            return Ok(products);
        }

        //[HttpPut("add-review")]
        //public async Task<IActionResult> AddReview([FromBody] UpdateReviewCommand command)
        //{
        //    var (review, userName) = await Mediator.Send(command);

        //    if (review == null || userName == null)
        //        return NotFound(new { Message = "Product not found or user not authenticated." });

        //    return Ok(new { Review = review, UserName = userName });
        //}

        [HttpPost("create-review")]
        public async Task<IActionResult> CreateReview(CreateReviewDto dto)
        {
            // Find the product by its name
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Name == dto.ProductName);
            if (product == null)
                return NotFound($"Product with name '{dto.ProductName}' not found.");

            // Find the user in the 'users' table
            var user = await _context.Usesrs
                .FirstOrDefaultAsync(u => u.id == dto.UserId); // Assuming `dto.UserId` is an `int`
            if (user == null)
                return NotFound($"User with ID '{dto.UserId}' not found.");

            // Create a new review
            var review = new ProductReview
            {
                UserId = user.id,
                ProductId = product.Id,
                ReviewText = dto.ReviewText,
            };

            // Add the review to the database
            _context.ProductReviews.Add(review);
            await _context.SaveChangesAsync();

            // Return the response with user name and review text
            var response = new
            {
                UserName = user.name,
                ReviewText = review.ReviewText
            };

            return Ok(response);
        }


        [HttpGet("reviews/{productName}")]
        public async Task<IActionResult> GetReviews(string productName)
        {
            var reviews = await (
                from review in _context.ProductReviews
                join user in _context.Usesrs on review.UserId equals user.id
                join product in _context.Products on review.ProductId equals product.Id
                where product.Name == productName
                select new ReviewResponseDto
                {
                    
                    UserName = user.name,
                    ReviewText = review.ReviewText
                    
                }
            ).ToListAsync();

            if (!reviews.Any())
                return NotFound($"No reviews found for product with name '{productName}'.");

            return Ok(reviews);
        }



    }


    public class ProductDto
    {
        public string Name { get; set; }
       
        public decimal Price { get; set; }
       
        public string ImageFile { get; set; }
    }

    public class CreateReviewDto
    {
        public string ProductName { get; set; }
        public int UserId { get; set; }
        public string ReviewText { get; set; }
    }
    public class ReviewResponseDto
    {
       
        public string UserName { get; set; }
        public string ReviewText { get; set; }
        
    }

}
