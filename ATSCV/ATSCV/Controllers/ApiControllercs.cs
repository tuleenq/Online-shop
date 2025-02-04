using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ATS_CV.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiControllercs : ControllerBase
    {
        private ISender _mediator = null;
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    }
}
