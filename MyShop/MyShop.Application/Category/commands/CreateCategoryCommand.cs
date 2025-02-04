using AutoMapper;
using MediatR;
using MyShop.Domain.Entities;
using MyShop.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class CreateCategoryCommand : IRequest<Ctegory>
{
    public string Name { get; set; }
  
}
public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, Ctegory>
{
    private readonly ApplicationDbContext _context;
    private IMapper _mapper;
    public CreateCategoryHandler(ApplicationDbContext context, IMapper maper)
    {
        _context = context;
        _mapper = maper;
    }

    public async Task<Ctegory> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        Ctegory cat = _mapper.Map<Ctegory>(request);

        _context.category.Add(cat);
        await _context.SaveChangesAsync(cancellationToken);

        return cat;
    }
}

