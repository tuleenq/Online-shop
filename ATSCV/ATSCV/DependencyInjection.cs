using ATS.Infrastructure;
using ATS.Application;

namespace ATS_CV
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddShop(this IServiceCollection service)
        {
            service.AddApplication();
            service.AddInfrastructure();

            return service;
        }
    }
}
