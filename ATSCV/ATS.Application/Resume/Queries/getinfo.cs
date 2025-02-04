using ATS.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ATS.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyShop.Application.Category.Queries
{
   
    public class getinfo : IRequest<Resume>
    {
        public int Id { get; set; }
    }

    
    public class getinfoHandler : IRequestHandler<getinfo, Resume>
    {
        private readonly ApplicationDbContext _context;

        public getinfoHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Resume> Handle(getinfo request, CancellationToken cancellationToken)
        {
            
            var resume = await _context.Resumes
                .Include(r => r.WorkExperiences) 
                .Include(r => r.EducationDetails)
                .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

            if (resume == null)
            {
                
                return null; 
            }

            return resume;
        }
    }
}
