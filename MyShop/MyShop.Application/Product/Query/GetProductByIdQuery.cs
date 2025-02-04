using MediatR;
using MyShop.Domain.Entities;
using MyShop.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GetProductByIdRequest
{
    public int Id { get; set; }
}
public record GetProductByIdQuery(int Id) : IRequest<Product>;

public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Product>
{
    private readonly ApplicationDbContext _context;

    public GetProductByIdHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Products.FindAsync(new object[] { request.Id }, cancellationToken);
    }
}

