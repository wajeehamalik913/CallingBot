using System;

namespace Common.Exceptions
{
    public class EntityForbiddenException : Exception
    {
        public readonly int Code;
        public EntityForbiddenException(int code, string message) : base(message)
        {
            this.Code = code;
        }
    }
}
