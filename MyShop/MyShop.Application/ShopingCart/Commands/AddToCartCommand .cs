using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyShop.Domain.Entities;
using MyShop.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AddToCartCommand : IRequest<ShoppingCart>
{
    public int UserId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
}
public class AddToCartHandler : IRequestHandler<AddToCartCommand, ShoppingCart>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    public AddToCartHandler(ApplicationDbContext context,IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ShoppingCart> Handle(AddToCartCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products
        .FirstOrDefaultAsync(p => p.Name == request.ProductName, cancellationToken);

        if (product == null)
            throw new ArgumentException("Product not found");

        // Find the shopping cart item for the given user and product
        ShoppingCart cartItem = await _context.ShoppingCarts
            .FirstOrDefaultAsync(c => c.UserId == request.UserId && c.ProductId == product.Id, cancellationToken);

        if (cartItem == null)
        {
            // Map the request to ShoppingCart, and assign the ProductId explicitly
            cartItem = _mapper.Map<ShoppingCart>(request);
            cartItem.ProductId = product.Id;  // Assign the correct ProductId

            await _context.ShoppingCarts.AddAsync(cartItem, cancellationToken);
        }
        else
        {
            cartItem.Quantity += request.Quantity;
        }

        await _context.SaveChangesAsync(cancellationToken);
        return cartItem;
    }
}