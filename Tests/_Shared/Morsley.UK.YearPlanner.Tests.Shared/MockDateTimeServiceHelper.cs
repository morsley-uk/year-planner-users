using Morsley.UK.YearPlanner.Users.Domain.Interfaces;
using System;

namespace Morsley.UK.YearPlanner.Users.Tests.Shared
{
    public class MockDateTimeServiceHelper
    {
        public static IDateTimeService Create(DateTime now)
        {
            var service = new MockDateTimeService();
            service.SetDateTimeUtcNow(now);
            return service;
        }
    }
}