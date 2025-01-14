using DesignPatternsDemo.AbstractFactoryPattern.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatternsDemo.AbstractFactoryPattern
{
    public static class RegisterAbstractFactoryClasses
    {
        public static IServiceCollection RegisterAbstractFactoryClass(IServiceCollection services)
        {
            services.AddScoped<ICountryFactory, USACountryFactory>();
            services.AddScoped<ICountryFactory, IndiaCountryFactory>();
            return services;
        }
    }
}
