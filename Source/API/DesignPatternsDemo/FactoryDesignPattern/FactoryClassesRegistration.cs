using DesignPatternsDemo.FactoryDesignPattern.demo1;
using DesignPatternsDemo.FactoryDesignPattern.demo1.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace DesignPatternsDemo.FactoryDesignPattern
{
    public static class FactoryClassesRegistration
    {
        public static IServiceCollection AddFactoryPatternServices(this IServiceCollection services)
        {
            services.AddScoped<IAddressFormatFactory, AddressFormatFactory>();
            services.AddScoped<MPAddress>();
            services.AddScoped<UPAddress>();
            return services;
        }
    }
}
