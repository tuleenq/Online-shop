using MediatR;
using MyShop.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using MyShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class ViewProductCommand : IRequest<IEnumerable<Product>>
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public int CtegoryId { get; set; }


}



public class GetAllProductsHandler : IRequestHandler<ViewProductCommand, IEnumerable<Product>>
{
    private readonly ApplicationDbContext _context;

    public GetAllProductsHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> Handle(ViewProductCommand request, CancellationToken cancellationToken)
    {
        return await _context.Products.Include(x=>x.Ctegory)
       .Select(p => new Product
       {
           Id = p.Id,
           Name = p.Name,
           Description = p.Description,
           Price = p.Price,
           StockQuantity = p.StockQuantity,
           CtegoryId = p.CtegoryId,
           Ctegory=p.Ctegory
       })
       .ToListAsync(cancellationToken);
    }
   
}