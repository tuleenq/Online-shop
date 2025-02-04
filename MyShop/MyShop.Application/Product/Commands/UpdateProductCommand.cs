using AutoMapper;
using MediatR;
using MyShop.Domain.Entities;
using MyShop.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UpdateProductCommand : IRequest<Product>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    
    public int CtegoryId { get; set; }
}
public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Product>
{
    private readonly ApplicationDbContext _context;
    private IMapper _mapper;
    public UpdateProductHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Product> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        

        var existingProduct = await _context.Products.FindAsync(request.Id , cancellationToken);

        if (existingProduct == null) return null;
        var quntity = existingProduct.StockQuantity;
        _mapper.Map(request, existingProduct);
       
        
    
        await _context.SaveChangesAsync(cancellationToken);
        return existingProduct;
    }
}