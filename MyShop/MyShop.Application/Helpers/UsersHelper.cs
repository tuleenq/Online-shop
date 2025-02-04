using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;

namespace MyShop.Application.Helpers
{
    public class UsersHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserNameFromSession()
        {
            // Retrieve the token from the session
            var token = _httpContextAccessor.HttpContext.Session.GetString("AuthToken");
            if (string.IsNullOrEmpty(token))
                return null;

            // Parse the JWT token
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Extract the 'sub' claim (username)
            var usernameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);

            return usernameClaim?.Value;
        }
    }
}
