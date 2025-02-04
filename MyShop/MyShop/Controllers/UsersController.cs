using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyShop.Controllers;
using MyShop.Domain.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MyShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ApiControllercs
    {


        [HttpPost("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers([FromBody] GetAllUsersQuery query)
        {
            var users = await Mediator.Send(query);
            return Ok(users);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("getById")]
        public async Task<ActionResult<User>> GetUserById([FromQuery] int id)
        {
            var query = new GetUserByIdQuery { Id = id };
            var user = await Mediator.Send(query);
            if (user == null) return NotFound("User not found.");
            return Ok(user);
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            if (command == null)
                return BadRequest("Invalid user data.");

            var createdUser = await Mediator.Send(command);

            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.id }, createdUser);
        }
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand command)
        {
            var updatedUser = await Mediator.Send(command);

            return Ok(updatedUser);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteUserCommand command)
        {
            var result = await Mediator.Send(command);

            return NoContent();
        }
    }
}
