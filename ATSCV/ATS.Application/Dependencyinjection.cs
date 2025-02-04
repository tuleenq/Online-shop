using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
namespace ATS.Application
{
    public static class Dependencyinjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection service)
        {

            service.AddMediatR(Assembly.GetExecutingAssembly());
            service.AddAutoMapper(Assembly.GetExecutingAssembly());
            return service;
        }
    }
}
