using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ATS_CV.MiddleWare
{
    public class TokenValidationFilter : IActionFilter
    {
        private readonly IConfiguration _configuration;

        public TokenValidationFilter(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var token = context.HttpContext.Request.Headers["Authorization"].ToString()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token) || !ValidateAccessToken(token))
            {
                context.HttpContext.Response.StatusCode = 401; 
                context.Result = new Microsoft.AspNetCore.Mvc.JsonResult(new { message = "Unauthorized" });
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        private bool ValidateAccessToken(string token)
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
