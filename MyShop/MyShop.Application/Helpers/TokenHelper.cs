using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyShop.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Application.Helpers
{
    public class TokenHelper
    {
        public async Task<string> GenerateAccessToken(IdentityUser user, IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            var userRoles = await userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };


            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("JWT signing key is missing in the configuration.");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task<string?> RefreshAccessTokenIfNeeded(
            string refreshToken, ApplicationDbContext context,
            IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            var user = context.Usesrs.FirstOrDefault(u => u.RefreshToken == refreshToken);
            if (user == null) return null;

            var identityUser = await userManager.FindByNameAsync(user.name);
            if (identityUser == null) return null;

            return await GenerateAccessToken(identityUser, configuration, userManager);
        }
    }
}
