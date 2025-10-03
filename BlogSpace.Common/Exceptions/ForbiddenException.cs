using System;

namespace BlogSpace.Common.Exceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException()
            : base("Action is forbidden.") { }

        public ForbiddenException(string message)
            : base(message) { }
    }
}
