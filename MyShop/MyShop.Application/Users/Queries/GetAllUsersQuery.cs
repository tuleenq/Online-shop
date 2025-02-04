using MediatR;
using Microsoft.EntityFrameworkCore;
using MyShop.Domain.Entities;
using MyShop.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GetAllUsersQuery : IRequest<IEnumerable<User>>
{
    
}
public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<User>>
{
    private readonly ApplicationDbContext _context;

    public GetAllUsersHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Usesrs.AsQueryable();

        return query;
    }
}