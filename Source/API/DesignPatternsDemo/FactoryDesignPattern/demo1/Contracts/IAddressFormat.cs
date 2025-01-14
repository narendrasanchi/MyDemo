using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatternsDemo.FactoryDesignPattern.demo1.Contracts
{
    public interface IAddressFormat
    {
        Task<string> FormatAddress();
    }
}
