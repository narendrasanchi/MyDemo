using DesignPatternsDemo.FactoryDesignPattern.demo1.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatternsDemo.AbstractFactoryPattern.Contracts
{
    public interface ICountryFactory
    {
        public IAddressFormat  CreateState(string stateName);
    }
}
