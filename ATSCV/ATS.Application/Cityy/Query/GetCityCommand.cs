using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATS.Domain.Entities;
using ATS.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace ATS.Application.Cityy.Query
{
    public class GetCitiesByCountryIdQuery : IRequest<List<CityDto>>
    {
        public int CountryId { get; set; }

        public GetCitiesByCountryIdQuery(int countryId)
        {
            CountryId = countryId;
        }
    }
    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class GetCitiesByCountryIdQueryHandler : IRequestHandler<GetCitiesByCountryIdQuery, List<CityDto>>
    {
        private readonly ApplicationDbContext _context;

        public GetCitiesByCountryIdQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CityDto>> Handle(GetCitiesByCountryIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Cities
                .Where(c => c.CountryId == request.CountryId)
                .Select(c => new CityDto { Id = c.Id, Name = c.Name })
                .ToListAsync(cancellationToken);
        }
    }
}
