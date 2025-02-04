using MediatR;
using MyShop.Domain.Entities;
using MyShop.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GetUserByIdQuery : IRequest<User>
{
    public int Id { get; set; }
}
public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, User>
{
    private readonly ApplicationDbContext _context;

    public GetUserByIdHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Usesrs.FindAsync(request.Id);
        if (user == null) return null;

        return user;
       
    }
}