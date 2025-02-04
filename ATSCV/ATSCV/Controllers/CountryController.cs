using ATS.Application.Countrys.Commands;
using ATS.Application.Countrys.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ATS_CV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : ApiControllercs
    {
        [HttpPost("AddCountry")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCountryCommand command)
        {
            if (command == null)
                return BadRequest("Invalid user data.");

            var createdCategory = await Mediator.Send(command);

            return CreatedAtAction(nameof(CreateCategory), new { id = createdCategory.Id }, createdCategory);
        }
        [HttpGet("GetCountries")]
        public async Task<ActionResult> Getcountries()
        {
            var products = await Mediator.Send(new GetCountrQuery());
            return Ok(products);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            
                var command = new DeleteCountryCommand(id);
                await Mediator.Send(command);
                return NoContent(); 
            
        }
    }
}
