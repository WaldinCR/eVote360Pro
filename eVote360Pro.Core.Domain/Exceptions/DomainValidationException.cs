using System;

namespace eVote360Pro.Core.Domain.Exceptions
{
    public class DomainValidationException : Exception
    {
        public DomainValidationException(string message) : base(message) { }
    }
}