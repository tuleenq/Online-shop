using ATS.Application.Helpers;
using ATS.Domain.Entities;
using ATS.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


public class LoginUserCommand : IRequest<IActionResult>
{
    public string? Email { get; set; }
    public string Password { get; set; }
}

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, IActionResult>
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly TokenHelper _tokenHelper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<LoginUserCommandHandler> _logger;
    private readonly UserHelper _userH;
    public LoginUserCommandHandler(
        UserManager<IdentityUser> userManager,
        IConfiguration configuration,
        ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor,
        TokenHelper tokenHelper,
        ILogger<LoginUserCommandHandler> logger, UserHelper userH)
    {
        _userManager = userManager;
        _configuration = configuration;
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _tokenHelper = tokenHelper;
        _logger = logger;
        _userH = userH;

    }

    public async Task<IActionResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        var password = await _userManager.CheckPasswordAsync(user, request.Password);
        if (user != null && password)
        {
        
            var RefreshTokenguid = Guid.NewGuid().ToString();
            var userid = await _userH.GetUserIdByNameAsync(user.UserName);
            if (userid.HasValue)
            {
                var token = await _tokenHelper.GenerateAccessToken(user, (int)userid, _userManager);
                var userdb = await _context.Users.FindAsync(userid);
                if (userdb != null)
                {
                    userdb.RefreshToken = RefreshTokenguid;
                    await _context.SaveChangesAsync();
                    _httpContextAccessor.HttpContext.Session.SetString("AuthToken", token);
                    _httpContextAccessor.HttpContext.Session.SetString("UserName", user.UserName);


                    return new OkObjectResult(new { Token = token, RefrshToken = RefreshTokenguid });
                }
            }

        }
        return new UnauthorizedResult();

    }
}