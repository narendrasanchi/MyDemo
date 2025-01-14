using DesignPatternsDemo.AbstractFactoryPattern.Contracts;
using DesignPatternsDemo.FactoryDesignPattern.Constants;
using DesignPatternsDemo.FactoryDesignPattern.demo1;
using DesignPatternsDemo.FactoryDesignPattern.demo1.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatternsDemo.AbstractFactoryPattern
{
    public sealed class USACountryFactory : ICountryFactory
    {
        public IAddressFormat CreateState(string stateName)
        {
            switch (stateName)
            {
                case Constants.Calefornia:
                    return new CaliforniaAddress();
                case Constants.Texas:
                    return new TexasAddress();
                default:
                    throw new ArgumentException("State not found");
            }
        }
    }
}
