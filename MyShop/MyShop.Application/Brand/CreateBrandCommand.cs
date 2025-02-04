using AutoMapper;
using MediatR;
using MyShop.Domain.Entities;
using MyShop.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CreateBrandCommand : IRequest<Brand>
{
    public string Name { get; set; }

}
public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, Brand>
{
    private readonly ApplicationDbContext _context;
    private IMapper _mapper;
    public CreateBrandCommandHandler(ApplicationDbContext context, IMapper maper)
    {
        _context = context;
        _mapper = maper;
    }

    public async Task<Brand> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
    {
        Brand br = _mapper.Map<Brand>(request);

        _context.Brands.Add(br);
        await _context.SaveChangesAsync(cancellationToken);

        return br;
    }
}
