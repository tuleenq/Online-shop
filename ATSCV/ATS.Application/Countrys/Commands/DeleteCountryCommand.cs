using ATS.Domain.Entities;
using ATS.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ATS.Application.Countrys.Commands
{
    public class DeleteCountryCommand : IRequest<Country>
    
    {
        public int Id { get; set; }
        public DeleteCountryCommand(int id)
        {
            Id = id;
        }
    }
    public class DeleteCountryHandler : IRequestHandler<DeleteCountryCommand,Country>
    {
        private readonly ApplicationDbContext _context;

        public DeleteCountryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Country> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
        {
            var country = await _context.countries
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (country == null)
            {
                throw new KeyNotFoundException($"Country with Id {request.Id} not found.");
            }

            _context.countries.Remove(country);
            await _context.SaveChangesAsync(cancellationToken);

            return country; 
        }


    }
}
