using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace MyShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Authentication : ApiControllercs
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            var result = await Mediator.Send(command);

            if (string.IsNullOrWhiteSpace(command.Email) || string.IsNullOrWhiteSpace(command.Password))
            {
                throw new Exception("Username or password is missing.");
            }
            return result;
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        { 
            return await Mediator.Send(command);
        }
        
    }
}
