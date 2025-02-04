using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyShop.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Infrastructure
{
    public static class DependencyInjuction
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection service)
        {
            service.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer("Server=DESKTOP-877QPHE\\SQLEXPRESS;Database=TestDb6;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true;");
            });
            return service;
            
        }
    }
}
