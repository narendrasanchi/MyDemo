using DesignPatternsDemo.FactoryDesignPattern.demo1.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FactoryController : ControllerBase
    {
        private readonly IAddressFormatFactory _addressFormatFactory;
        public FactoryController(IAddressFormatFactory addressFormatFactory)
        {
            _addressFormatFactory = addressFormatFactory;
        }
        private readonly ILogger<WeatherForecastController> _logger;
        [HttpGet(Name = "factory")]
        public Task<string> factory(string state)
        {
            return _addressFormatFactory.getAddressServiceObject(state).FormatAddress();
        }
    }
} 
