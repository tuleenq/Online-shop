using ATS.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ATS.Application.Helpers
{
    public class TokenHelper { 
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        public TokenHelper(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<string> GenerateAccessToken(IdentityUser user,int userid, UserManager<IdentityUser> userManager)
        {
            
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userid.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GetUserIdFromToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new UnauthorizedAccessException("Token is missing.");

            var handler = new JwtSecurityTokenHandler();
            if (!handler.CanReadToken(token))
                throw new UnauthorizedAccessException("Invalid token.");

            var jwtToken = handler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);

            if (userIdClaim == null)
                throw new UnauthorizedAccessException("Invalid token: 'sub' claim is missing.");

            return userIdClaim.Value;
        }
        private static bool IsBase64UrlEncoded(string token)
        {
            
            var parts = token.Split('.');
            return parts.Length == 3 && parts.All(IsBase64Url);
        }

        private static bool IsBase64Url(string input)
        {
            return !string.IsNullOrEmpty(input) && !input.Contains('+') && !input.Contains('/') && !input.Contains('=');
        }

        public async Task<string?> RefreshAccessTokenIfNeeded(
            string refreshToken, ApplicationDbContext context,
            IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            var user = context.Users.FirstOrDefault(u => u.RefreshToken == refreshToken && u.RefreshTokenExpiryTime > DateTime.UtcNow);
            if (user == null) return null;

            if (user == null) return null;

            var identityUser = await userManager.FindByNameAsync(user.Name);
            if (identityUser == null) return null;

            return await GenerateAccessToken(identityUser, user.Id, userManager);
        }
        public bool ValidateAccessToken(string token)
        {
            if (string.IsNullOrEmpty(token))
                return false;

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

                var parameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                tokenHandler.ValidateToken(token, parameters, out _);
                return true; 
            }
            catch
            {
                return false; 
            }
        }


    }
}
