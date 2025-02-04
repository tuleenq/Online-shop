using MediatR;
using MyShop.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DeleteUserCommand : IRequest<bool>
{
    public int Id { get; set; }
}
public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly ApplicationDbContext _context;

    public DeleteUserHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Usesrs.FindAsync(request.Id);
        if (user == null) return false;

        _context.Usesrs.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
