using ATS.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Application.Countrys.Queries
{
    public class GetCountrQuery : IRequest<List<CountryDto>>
    {
        public string Id { get; set; }
        public string Name { get; set; }


    }



    public class GetCountryHandler : IRequestHandler<GetCountrQuery, List<CountryDto>>
    {
        private readonly ApplicationDbContext _context;

        public GetCountryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CountryDto>> Handle(GetCountrQuery request, CancellationToken cancellationToken)
        {
            var countries = await _context.countries
         .Select(c => new CountryDto
         {
             Id = c.Id,
             Name = c.Name,
             Code=c.Code
         })
         .ToListAsync();
            return countries;
        }

    }
    public class CountryDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Code { get; set; }
    }
}
