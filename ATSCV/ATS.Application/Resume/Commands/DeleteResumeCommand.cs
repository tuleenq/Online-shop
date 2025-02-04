using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATS.Infrastructure.Data;
using MediatR;

namespace ATS.Application.Resumes;

public class DeleteResumeCommand : IRequest<bool>
    {
        public int Id { get; set; }

       
    }


    public class DeleteResumeCommandHandler : IRequestHandler<DeleteResumeCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public DeleteResumeCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteResumeCommand request, CancellationToken cancellationToken)
        {
            var resume = await _context.Resumes.FindAsync(request.Id);
            if (resume == null)
            {
                return false; 
            }

            _context.Resumes.Remove(resume);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }

