using System;

namespace Morsley.UK.YearPlanner.Users.Domain.Exceptions
{
    public class DomainModelIdException : Exception
    {
        public DomainModelIdException(string message) : base(message) { }
    }
}