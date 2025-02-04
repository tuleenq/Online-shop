using MediatR;
using Microsoft.EntityFrameworkCore;
using MyShop.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RemoveFromCartCommand : IRequest<bool>
{
    public int UserId { get; set; }
    public string ProductName { get; set; }

    public RemoveFromCartCommand(int userId, string productName)
    {
        UserId = userId;
        ProductName = productName;
    }
}
public class RemoveFromCartHandler : IRequestHandler<RemoveFromCartCommand, bool>
{
    private readonly ApplicationDbContext _context;

    public RemoveFromCartHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(RemoveFromCartCommand request, CancellationToken cancellationToken)
    {
        var cartItem = await _context.ShoppingCarts
            .FirstOrDefaultAsync(c => c.UserId == request.UserId && c.ProductName == request.ProductName, cancellationToken);

        if (cartItem == null) return false;

        _context.ShoppingCarts.Remove(cartItem);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}