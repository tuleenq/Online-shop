using ATS.Domain.Entities;
using ATS.Infrastructure.Data;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Application.Cityy.Commands
{
    public class CreateCityCommand : IRequest<int>
    {
        public string Name { get; set; }
        public int CountryId { get; set; }

        public CreateCityCommand(string name, int countryId)
        {
            Name = name;
            CountryId = countryId;
        }
    }
    public class CreateCityCommandHandler : IRequestHandler<CreateCityCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public CreateCityCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateCityCommand request, CancellationToken cancellationToken)
        {
           
            var countryExists = await _context.countries.AnyAsync(c => c.Id == request.CountryId, cancellationToken);
            if (!countryExists)
            {
                throw new InvalidOperationException($"Country with Id {request.CountryId} does not exist.");
            }

            var city = new City
            {
                Name = request.Name,
                CountryId = request.CountryId
            };

            _context.Cities.Add(city);
            await _context.SaveChangesAsync(cancellationToken);

            return city.Id; 
        }
    }
}
