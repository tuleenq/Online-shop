using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATS.Domain.Entities;
using ATS.Infrastructure.Data;
using AutoMapper;
 public class CreateTemplateCommand:IRequest<Template>
 {
    public string Name { get; set; }
    public string HtmlContent { get; set; }

  }
public class CreateCategoryHandler : IRequestHandler<CreateTemplateCommand, Template>
{
    private readonly ApplicationDbContext _context;
    private IMapper _mapper;
    public CreateCategoryHandler(ApplicationDbContext context, IMapper maper)
    {
        _context = context;
        _mapper = maper;
    }

    public async Task<Template> Handle(CreateTemplateCommand request, CancellationToken cancellationToken)
    {
        Template cat = _mapper.Map<Template>(request);

        _context.Templates.Add(cat);
        await _context.SaveChangesAsync(cancellationToken);

        return cat;
    }
}

