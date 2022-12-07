using System;

namespace Common.Models
{
    public class FailedDetailedResponseContent : FailedResponseContent
    {
        public Exception ErrorDetails { get; set; }
    }
}
