using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

public class ValidateParametersAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var command = context.ActionArguments.Values.FirstOrDefault(v => v is AddToCartCommand) as AddToCartCommand;

        if (command == null)
        {
            context.Result = new BadRequestObjectResult("Invalid request data.");
            return;
        }

        // Validate UserId > 1
        if (command.UserId <= 0)
        {
            context.Result = new BadRequestObjectResult("User ID must be greater than 1.");
            return;
        }

        // Validate ProductName is not null
        if (string.IsNullOrEmpty(command.ProductName))
        {
            context.Result = new BadRequestObjectResult("Product name cannot be null or empty.");
            return;
        }

        // Validate Quantity > 1
        if (command.Quantity <= 0)
        {
            context.Result = new BadRequestObjectResult("Quantity must be greater than 1.");
            return;
        }

        base.OnActionExecuting(context);
    }
}
