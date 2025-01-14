using Common.Enum;
using System.Text.Json.Serialization;

namespace Common
{
    public class ResponseMessage
    {
        private readonly string _code;
        private string _message;
        private readonly Severity _severity;
        private readonly string[] _parameters;
        private int _errorcode;

        public ResponseMessage(string code, string message, Severity severity, params string[] parameters)
        {
            _code = code;
            _message = message;
            _severity = severity;
            _parameters = parameters == null || !parameters.Any() ? null : parameters;
        }

        [JsonConstructor]
        public ResponseMessage(string code, string message, Severity severity, int errorcode, params string[] parameters)
        {
            _code = code;
            _message = message;
            _severity = severity;
            _parameters = parameters == null || !parameters.Any() ? null : parameters;
            _errorcode = errorcode;
        }

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public string Code
        {
            get { return _code; }
        }

        public Severity Severity
        {
            get { return _severity; }
        }

        public string[] Parameters
        {
            get { return _parameters; }
        }

        public int ErrorCode
        {
            get { return _errorcode; }
        }

        public static ResponseMessage New(string code, string message, Severity severity, params string[] parameters)
        {
            if (parameters == null || !parameters.Any())
                return new ResponseMessage(code, message, severity);
            return new ResponseMessage(code, string.Format(message, parameters), severity, parameters);
        }

        public override string ToString()
        {
            return string.Format("{0}::{1} : {2}", _severity, _code, _message);
        }
    }

    public static class ResponseMessageExtension
    {
        public static ResponseMessage FormatMessage(this ResponseMessage responseMessage, params object[] parameters)
        {
            if (parameters == null || !parameters.Any()) return responseMessage;
            responseMessage.Message = string.Format(responseMessage.Message, parameters);
            return responseMessage;
        }
    }
}
