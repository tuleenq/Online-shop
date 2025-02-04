using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyShop.Application.Category.Queries;
using MyShop.Domain.Entities;

namespace MyShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ApiControllercs
    {
        [HttpPost("AddCategory")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand command)
        {
            if (command == null)
                return BadRequest("Invalid user data.");

            var createdCategory = await Mediator.Send(command);

            return CreatedAtAction(nameof(CreateCategory), new { id = createdCategory.Id }, createdCategory);
        }
        [HttpGet("GetCategories")]
        public async Task<ActionResult> Getcategories()
        {
            var products = await Mediator.Send(new ViewCategoryCommand());
            return Ok(products);
        }
    }
}
