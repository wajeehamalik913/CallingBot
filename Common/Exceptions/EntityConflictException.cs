using System;

namespace Common.Exceptions
{
    public class EntityConflictException : Exception
    {
        public readonly int Code;
        public EntityConflictException(int code, string message) : base(message)
        {
            this.Code = code;
        }
    }
}
