using System;

namespace Common.Exceptions
{
    public class EntityMissingException : Exception
    {
        public readonly int Code;
        public EntityMissingException(int code, string message) : base(message)
        {
            this.Code = code;
        }
    }
}
