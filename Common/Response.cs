using Common.Contracts;
using Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Common
{
    public class Response<T> : IResponse
    {
        private T _model;

        public T Model
        { get { return _model; } }

        private ResponseCode _code;

        public ResponseCode Code
        { get { return _code; } }

        private ResponseMessage[] _messages;

        public ResponseMessage[] Messages
        { get { return _messages; } }

        public bool IsSuccess
        { get { return (_code == ResponseCode.Ok); } }

        public Response(T model, ResponseCode code = ResponseCode.Ok)
        {
            _model = model;
            _code = code;
            _messages = null;
        }

        [JsonConstructor]
        public Response(T model, ResponseCode code, ResponseMessage[] messages)
        {
            _model = model;
            _code = code;
            _messages = (messages == null || !messages.Any()) ? null : messages;
        }

        public Response(T model, ResponseCode code, List<ResponseMessage> messages)
        {
            _model = model;
            _code = code;
            _messages = (messages == null || !messages.Any()) ? null : messages.ToArray();
        }

        public Response(T model, ResponseCode code, ResponseMessage messages)
        {
            _model = model;
            _code = code;
            _messages = messages == null ? null : new[] { messages };
        }

       

        public void AddMessage(ResponseMessage message)
        {
            if (message == null || string.IsNullOrWhiteSpace(message.Code) || string.IsNullOrWhiteSpace(message.Message)) return;
            if (_messages == null)
            {
                _messages = new[] { message };
            }
            else
            {
                var list = _messages.ToList();
                list.Add(message);
                _messages = list.ToArray();
            }
        }

        public override string ToString()
        {
            if (Messages == null) return string.Empty;
            var messages = new StringBuilder(string.Empty);
            foreach (var message in Messages)
            {
                messages.AppendLine(message.ToString());
            }
            return messages.ToString();
        }

        public string GetMessage()
        {
            if (Messages == null) return string.Empty;
            var messages = new StringBuilder(string.Empty);
            foreach (var message in Messages)
            {
                messages.AppendLine(message.Message);
            }
            return messages.ToString();
        }
    }
}
