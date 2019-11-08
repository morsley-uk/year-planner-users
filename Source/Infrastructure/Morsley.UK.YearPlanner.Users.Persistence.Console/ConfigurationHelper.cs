using System;
using Morsley.UK.YearPlanner.Users.Domain.Interfaces;

namespace Morsley.UK.YearPlanner.Users.Persistence.Console
{
    public class ConfigurationHelper
    {
        private readonly IEnvironmentService _environmentService;

        public ConfigurationHelper(IEnvironmentService environmentService)
        {
            _environmentService = environmentService;
        }

        public string GetConnectionString()
        {
            //var connectionString = Shared.EnvironmentService.GetEnvironmentVariableValueByKey(Shared.Constants.EnvironmentVariables.UsersPersistenceKey);
            var connectionString = _environmentService.GetVariable(Shared.Constants.EnvironmentVariables.UsersPersistenceKey);

            if (!string.IsNullOrEmpty(connectionString))
            {
                return connectionString;
            }

            throw new ConnectionStringException($"Could not get the connection string from the environment variable ({Shared.Constants.EnvironmentVariables.UsersPersistenceKey})!");
        }
    }
}