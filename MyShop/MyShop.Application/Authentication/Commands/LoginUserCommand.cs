using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyShop.Application.Helpers;
using MyShop.Domain.Entities;
using MyShop.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
    public class LoginUserCommand : IRequest<IActionResult>
    {
        public string Email { get; set; }
        public string Name {  get; set; }
        public string Password { get; set; }
       
        public string Address { get; set; }
        public string Number {  get; set; }

    }
public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, IActionResult>
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly TokenHelper _tokenService;
    private readonly UserHelper _userH;
    private readonly IConfiguration _configuration;
    public LoginUserCommandHandler(ApplicationDbContext context, UserManager<IdentityUser> userManager,
                              RoleManager<IdentityRole> roleManager,
                              IConfiguration configuration,
                              IHttpContextAccessor httpContextAccessor, ILogger<UserHelper> logger, TokenHelper tokenService, UserHelper userH) 
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _context = context;
        _tokenService = tokenService;
        _userH = userH;
    }
    private async Task EnsureRolesExist()
    {
        if (!await _roleManager.RoleExistsAsync("Admin"))
        {
            await _roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        if (!await _roleManager.RoleExistsAsync("Client"))
        {
            await _roleManager.CreateAsync(new IdentityRole("Client"));
        }
    }
    public async Task<IActionResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        await EnsureRolesExist();
        var user = new IdentityUser { UserName = request.Name, Email = request.Email };
        var result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            
            
            var newUser = new User
            {
                name = request.Name,
                address = request.Address,
                number = request.Number,
                         
                email = request.Email,     
                password = request.Password 

            };


            _context.Usesrs.Add(newUser);
            await _context.SaveChangesAsync();

            var accessToken = await _tokenService.GenerateAccessToken(user, _configuration, _userManager);

            return new OkObjectResult(new
            {
                AccessToken = accessToken,
                RefreshToken = newUser.RefreshToken
            });
        }

        return new BadRequestObjectResult(result.Errors);
    }
}

