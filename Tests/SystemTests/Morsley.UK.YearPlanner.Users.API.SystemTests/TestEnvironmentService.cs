using Morsley.UK.YearPlanner.Users.Domain.Interfaces;
using System;

namespace Morsley.UK.YearPlanner.Users.API.SystemTests
{
    public class TestEnvironmentService  : IEnvironmentService
    {
        public string GetVariable(string key)
        {
            throw new NotImplementedException();
        }

        public void SetVariable(string key, string value)
        {
            throw new NotImplementedException();
        }
    }
}
