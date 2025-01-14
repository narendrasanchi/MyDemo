using Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Contracts
{
    public interface IResponse
    {
        ResponseCode Code { get; }
        bool IsSuccess { get; }
        ResponseMessage[] Messages { get; }

        void AddMessage(ResponseMessage message);
    }
}
