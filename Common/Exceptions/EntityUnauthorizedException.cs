using System;

namespace Common.Exceptions
{
    public class EntityUnauthorizedException : Exception
    {
        public readonly int Code;
        public EntityUnauthorizedException(int code, string message) : base(message)
        {
            this.Code = code;
        }
    }
}
