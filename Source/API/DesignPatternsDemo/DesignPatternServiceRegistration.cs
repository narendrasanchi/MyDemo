using DesignPatternsDemo.FactoryDesignPattern;
using Microsoft.Extensions.DependencyInjection;

namespace DesignPatternsDemo
{
    public static class DesignPatternServiceRegistration
    {
        public static IServiceCollection AddDesignPatternServices(this IServiceCollection services)
        {
            services.AddFactoryPatternServices();
            return services;
        }
    }
}
