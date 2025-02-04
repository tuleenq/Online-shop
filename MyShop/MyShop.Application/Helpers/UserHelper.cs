﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyShop.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.JsonWebTokens;

namespace MyShop.Application.Helpers
{
    public class UserHelper
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserHelper> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public UserHelper(ApplicationDbContext context, ILogger<UserHelper> logger, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<int?> GetUserIdByNameAsync(string? Username=null)
        {
            var usernameClaim = _httpContextAccessor.HttpContext?.User.FindFirst(JwtRegisteredClaimNames.Sub);
            if (usernameClaim == null && Username==null)
            {
                _logger.LogWarning("User not authenticated.");
                return null;
            }

            string username = Username??usernameClaim.Value;


            var user = await _context.Usesrs
                .FirstOrDefaultAsync(u => u.name == username);

            return user?.id;
        }
    }
}
