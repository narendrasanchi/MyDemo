using DesignPatternsDemo.FactoryDesignPattern.demo1.Contracts;
using Microsoft.Extensions.DependencyInjection;
namespace DesignPatternsDemo.FactoryDesignPattern.demo1
{
    public class AddressFormatFactory : IAddressFormatFactory
    {
        IServiceProvider _serviceProvider;
        public AddressFormatFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public IAddressFormat getAddressServiceObject(string state)
        {
            return state switch
            {
                Constants.Constants.MP => _serviceProvider.GetRequiredService<MPAddress>(),
                Constants.Constants.UP => _serviceProvider.GetRequiredService<UPAddress>(),
            };
        }
    }
}
