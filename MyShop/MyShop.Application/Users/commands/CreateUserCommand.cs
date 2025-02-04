using AutoMapper;
using MediatR;
using MyShop.Domain.Entities;
using MyShop.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class CreateUserCommand : IRequest<User>
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string Number { get; set; }
    public string Role { get; set; }
    public string email { get; set; }
    public string password { get; set; }

}
public class CreateUserHandler : IRequestHandler<CreateUserCommand, User>
{
    private readonly ApplicationDbContext _context;
    private IMapper _mapper;
    public CreateUserHandler(ApplicationDbContext context, IMapper maper)
    {
        _context = context;
        _mapper = maper;    
    }

    public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        User user = _mapper.Map<User>(request);
        
        _context.Usesrs.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

       return user;
    }
}