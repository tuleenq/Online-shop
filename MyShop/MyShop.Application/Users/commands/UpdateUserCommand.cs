using AutoMapper;
using MediatR;
using MyShop.Domain.Entities;
using MyShop.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UpdateUserCommand : IRequest<User>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Number { get; set; }
}
public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, User>
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    public UpdateUserHandler(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<User> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        User user = _mapper.Map<User>(request);
        if (user == null) return null;
        await _context.SaveChangesAsync(cancellationToken);
        return user;
       
    }
}
