using System;

namespace Morsley.UK.YearPlanner.Users.Domain.Interfaces
{
    public interface IDateTimeService
    {
        DateTime GetDateTimeUtcNow();
    }
}