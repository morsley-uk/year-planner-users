using System;
using Morsley.UK.YearPlanner.Users.Domain.Interfaces;

namespace Morsley.UK.YearPlanner.Users.Infrastructure
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime GetDateTimeUtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}