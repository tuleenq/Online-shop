using IdentityServer4.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
using System.Threading;
using System.Threading.Tasks;


public class RegisterUserCommand : IRequest<IActionResult>
{


    public string Email { get; set; }
    public string Password { get; set; }

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, IActionResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly TokenHelper _tokenService;
        private readonly UserHelper _userH;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public RegisterUserCommandHandler(UserManager<IdentityUser> userManager,
                                  RoleManager<IdentityRole> roleManager,
                                  IConfiguration configuration,
                                  ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, ILogger<UserHelper> logger, TokenHelper tokenService, UserHelper userH)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
            _tokenService = tokenService;
            _userH = userH;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<IActionResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {

            var user = await _userManager.FindByEmailAsync(request.Email);
            var password = await _userManager.CheckPasswordAsync(user, request.Password);
            if (user != null && password)
            {
                var token = await _tokenService.GenerateAccessToken(user, _configuration, _userManager);
                var RefreshTokenguid = Guid.NewGuid().ToString();
                var userid = await _userH.GetUserIdByNameAsync(user.UserName);
                if (userid.HasValue)
                {
                    var userdb = await _context.Usesrs.FindAsync(userid);
                    if (userdb != null)
                    {
                        userdb.RefreshToken = RefreshTokenguid;
                        await _context.SaveChangesAsync();
                         if (_httpContextAccessor.HttpContext != null)
                         {
                             _httpContextAccessor.HttpContext.Session.SetString("AuthToken", token);
                             _httpContextAccessor.HttpContext.Session.SetString("UserName", user.UserName);
                          }
                        return new OkObjectResult(new
                        {
                            User = userdb,
                            Token = token
                        });
                    }
                }

            }

            return new UnauthorizedResult();
        }
    }
}