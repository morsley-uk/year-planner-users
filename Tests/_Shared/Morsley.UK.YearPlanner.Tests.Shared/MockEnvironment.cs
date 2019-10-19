using System.Collections.Generic;

namespace Morsley.UK.YearPlanner.Users.Tests.Shared
{
    public class MockEnvironment
    {
        private Dictionary<string, string> _mockEnvironment;

        public MockEnvironment()
        {
            _mockEnvironment = new Dictionary<string, string>();
        }

        public string GetVariable(string key)
        {
            return _mockEnvironment[key];
        }

        public void SetVariable(string key, string value)
        {
            if (_mockEnvironment.ContainsKey(key))
            {
                _mockEnvironment[key] = value;
            }
            else
            {
                _mockEnvironment[key] = value;
            }
        }
    }
}