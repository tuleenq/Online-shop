using Microsoft.AspNetCore.Mvc;

namespace MyShop.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ApiControllercs
    {
        [HttpPost("process-payment")]
        public async Task<IActionResult> ProcessPayment([FromBody] ProcessPaymentCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
 