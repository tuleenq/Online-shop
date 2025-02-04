using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using MyShop.Controllers;

namespace apiProject.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartController : ApiControllercs
    {

       
        [HttpPost("add")]
        [ValidateParameters]

        public async Task<IActionResult> AddToCart([FromBody] AddToCartCommand command)
        {
            var cartItem = await Mediator.Send(command);

            return Ok(cartItem);
        }



        [HttpPost("view")]
        public async Task<IActionResult> ViewCart([FromBody] ViewCartQuery query)
        {
            var cartItems = await Mediator.Send(query);

            if (!cartItems.Any())
                return NotFound("No items found in the cart.");

            return Ok(cartItems);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> RemoveFromCart([FromBody] RemoveFromCartCommand command)
        {
            var result = await Mediator.Send(command);

            return NoContent();
        }
    }
}
