using DesignPatternsDemo.AbstractFactoryPattern.Contracts;
using DesignPatternsDemo.FactoryDesignPattern.demo1.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatternsDemo.AbstractFactoryPattern
{
    public sealed class CountryAbstractFactory : ICountryFactory
    {
        public IAddressFormat CreateState(string stateName)
        {
            throw new NotImplementedException();
        }
      
    }
}
