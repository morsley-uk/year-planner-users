using System;

namespace Morsley.UK.YearPlanner.Users.Persistence.Console
{
    public class ConnectionStringException : Exception
    {
        public ConnectionStringException(string message) : base(message) {}
    }
}