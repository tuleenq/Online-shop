using ATS.Application.Cityy.Commands;
using ATS.Application.Cityy.Query;
using ATS.Application.Countrys.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ATS_CV.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CityController : Controller
    {
        private readonly IMediator _mediator;

        public CityController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("cities/{countryId}")]
        public async Task<IActionResult> GetCities(int countryId)
        {
            var query = new GetCitiesByCountryIdQuery(countryId);
            var cities = await _mediator.Send(query);
            return Json(cities);
        }
        [HttpPost("cities")]
        public async Task<IActionResult> CreateCity([FromBody] CreateCityCommand command)
        {
            try
            {
                var cityId = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetCities), new { countryId = command.CountryId }, new { Id = cityId });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
    }
}
