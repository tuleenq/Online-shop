using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyShop.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

public class ProcessPaymentCommand : IRequest<IActionResult>
{
    public int UserId { get; }  // Changed from UserName to UserId

    public ProcessPaymentCommand(int userId)
    {
        UserId = userId;
    }
}

public class ProcessPaymentHandler : IRequestHandler<ProcessPaymentCommand, IActionResult>
{
    private readonly ApplicationDbContext _context;

    public ProcessPaymentHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
    {
        // Check if the user exists based on the provided UserId
        var user = await _context.Usesrs
            .FirstOrDefaultAsync(u => u.id == request.UserId, cancellationToken); // Use UserId for lookup

        if (user == null)
        {
            return new NotFoundObjectResult(new { Message = $"User with ID '{request.UserId}' not found." });
        }

        // Retrieve the shopping cart items for the user
        var cartItems = await _context.ShoppingCarts
            .Include(c => c.Product)
            .Where(c => c.UserId == request.UserId)  // Use UserId for filtering the cart
            .ToListAsync(cancellationToken);

        if (!cartItems.Any())
        {
            return new BadRequestObjectResult(new { Message = "Shopping cart is empty." });
        }

        // Calculate the total amount of the shopping cart
        float totalAmount = (float)cartItems.Sum(item => item.Product.Price * item.Quantity);

        // Return the total amount in the response
        return new OkObjectResult(new { TotalAmount = totalAmount });
    }
}