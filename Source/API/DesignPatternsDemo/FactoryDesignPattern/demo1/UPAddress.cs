using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignPatternsDemo.FactoryDesignPattern.demo1.Contracts;

namespace DesignPatternsDemo.FactoryDesignPattern.demo1
{
    internal class UPAddress : IAddressFormat
    {
        public async Task<string> FormatAddress()
        {
            return "UP Address";
        }
    }
}
