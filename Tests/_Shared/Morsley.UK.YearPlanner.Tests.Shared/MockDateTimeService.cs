using Morsley.UK.YearPlanner.Users.Domain.Interfaces;
using System;

namespace Morsley.UK.YearPlanner.Users.Tests.Shared
{
    public class MockDateTimeService : IDateTimeService
    {
        private DateTime _dateTimeUtcNow = DateTime.MinValue;

        public void SetDateTimeUtcNow(DateTime dateTime)
        {
            _dateTimeUtcNow = dateTime;
        }

        public DateTime GetDateTimeUtcNow()
        {
            return _dateTimeUtcNow;
        }
    }
}