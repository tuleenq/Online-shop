using Microsoft.AspNetCore.Mvc;

namespace MyShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandController : ApiControllercs
    {
        [HttpPost("AddBrand")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateBrandCommand command)
        {
            if (command == null)
                return BadRequest("Invalid user data.");

            var createdBrand = await Mediator.Send(command);

            return CreatedAtAction(nameof(CreateCategory), new { id = createdBrand.Id }, createdBrand);
        }
    }
}
