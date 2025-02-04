using MyShop.Application;
using MyShop.Infrastructure;

namespace MyShop
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddShop(this IServiceCollection service)
        {
            service.AddApplication()
                   .AddInfrastructure();

            return service;
        }
    }
}
