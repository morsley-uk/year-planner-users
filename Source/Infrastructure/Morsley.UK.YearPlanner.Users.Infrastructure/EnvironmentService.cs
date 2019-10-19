using System;
using Morsley.UK.YearPlanner.Users.Domain.Interfaces;

namespace Morsley.UK.YearPlanner.Users.Infrastructure
{
    public class EnvironmentService : IEnvironmentService
    {
        public string GetVariable(string key)
        {
            return System.Environment.GetEnvironmentVariable(key);
        }

        public void SetVariable(string key, string value)
        {
            System.Environment.SetEnvironmentVariable(key, value);
        }
    }
}