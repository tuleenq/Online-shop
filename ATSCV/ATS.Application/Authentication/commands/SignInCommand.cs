using ATS.Domain.Entities;
using ATS.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ATS.Application.Helpers;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;


public class SignInCommand : IRequest<IActionResult>
{
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; }
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; }
    public string Password { get; set; }
    [RegularExpression("^(Admin|Client)$", ErrorMessage = "Role must be either Admin or Client")]
    public string Role { get; set; }
    public string Address { get; set; }
  

}
public class SigninUserCommandHandler : IRequestHandler<SignInCommand, IActionResult>
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly TokenHelper _tokenService;
    private readonly UserHelper _userH;
    private readonly IConfiguration _configuration;
    public SigninUserCommandHandler(ApplicationDbContext context, UserManager<IdentityUser> userManager,
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
    public async Task<IActionResult> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        await EnsureRolesExist();
        var user = new IdentityUser { UserName = request.Name, Email = request.Email };
        var result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            var role = string.IsNullOrEmpty(request.Role) ? "Client" : request.Role;
            await _userManager.AddToRoleAsync(user, role);
            var newUser = new Users
            {
                Name = request.Name,
                Role = role,
                Email = request.Email,
                Password = request.Password

            };


            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            var accessToken = await _tokenService.GenerateAccessToken(user, newUser.Id, _userManager);

            return new OkObjectResult(new
            {
                AccessToken = accessToken,
                RefreshToken = newUser.RefreshToken
            });
        }

        return new BadRequestObjectResult(result.Errors);
    }
}

