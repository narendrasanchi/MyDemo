using DesignPatternsDemo.FactoryDesignPattern.demo1.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatternsDemo.FactoryDesignPattern.demo1
{
    public sealed class CaliforniaAddress : IAddressFormat
    {
        public Task<string> FormatAddress()
        {
            return Task.FromResult("Calefornia Address.");
        }
    }
}
