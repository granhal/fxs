using System;
namespace interviewFXS.Exceptions
{
    public class FXSException : Exception
    {
        public string Code { get; set; }
        public FXSException() : base() { }
        public FXSException(string message) : base(message)
        {
            Code = "000";
        }
        public FXSException(string message, string code) : base(message)
        {
            Code = code;
        }
        public FXSException(string message, Exception inner, string code) : base(message, inner)
        {
            Code = code;
        }
    }
}
