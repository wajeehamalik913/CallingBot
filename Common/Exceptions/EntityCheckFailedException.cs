using System;

namespace Common.Exceptions
{
    public class EntityCheckFailedException : Exception
    {
        public readonly int Code;
        public EntityCheckFailedException(int code, string message) : base(message)
        {
            this.Code = code;
        }
    }
}
