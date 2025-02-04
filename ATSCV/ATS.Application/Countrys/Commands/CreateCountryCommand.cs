using ATS.Infrastructure.Data;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATS.Domain.Entities;

namespace ATS.Application.Countrys.Commands
{
    public class CreateCountryCommand : IRequest<Country>
    {
        public string Name { get; set; }
        public string Code { get; set; }



    }
    public class CreateCategoryHandler : IRequestHandler<CreateCountryCommand, Country>
    {
        private readonly ApplicationDbContext _context;
        private IMapper _mapper;
        public CreateCategoryHandler(ApplicationDbContext context, IMapper maper)
        {
            _context = context;
            _mapper = maper;
        }

        public async Task<Country> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
        {
            Country cat = _mapper.Map<Country>(request);

            _context.countries.Add(cat);
            await _context.SaveChangesAsync(cancellationToken);

            return cat;
        }
    }
}
