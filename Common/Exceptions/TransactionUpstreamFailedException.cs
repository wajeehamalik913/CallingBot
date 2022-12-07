using System;

namespace Common.Exceptions
{
    public class TransactionUpstreamFailedException : Exception
    {
        public readonly int Code;
        public readonly int ResponseCode;
        public TransactionUpstreamFailedException(int code, string message, int responseCode) : base(message)
        {
            this.Code = code;
            ResponseCode = responseCode;
        }
    }
}
