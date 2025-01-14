using DesignPatternsDemo.AbstractFactoryPattern.Contracts;
using DesignPatternsDemo.FactoryDesignPattern.Constants;
using DesignPatternsDemo.FactoryDesignPattern.demo1;
using DesignPatternsDemo.FactoryDesignPattern.demo1.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatternsDemo.AbstractFactoryPattern
{
    public class IndiaCountryFactory : ICountryFactory
    {
        public IAddressFormat CreateState(string stateName)
        {
            switch (stateName)
            {
                case Constants.MP:
                    return new MPAddress();
                case Constants.UP:
                    return new UPAddress();
                default:
                    throw new ArgumentException("State not found");
            }
        }
    }
}
