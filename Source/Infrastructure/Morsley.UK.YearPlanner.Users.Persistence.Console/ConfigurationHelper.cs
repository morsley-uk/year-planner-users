using System;

namespace Morsley.UK.YearPlanner.Users.Persistence.Console
{
    public class ConfigurationHelper
    {
        public static string GetConnectionString()
        {
            var connectionString = Shared.Environment.GetEnvironmentVariableValueByKey(Shared.Constants.EnvironmentVariables.UsersPersistenceKey);

            if (!string.IsNullOrEmpty(connectionString))
            {
                return connectionString;
            }

            throw new ConnectionStringException($"Could not get the connection string from the environment variable ({Shared.Constants.EnvironmentVariables.UsersPersistenceKey})!");
        }
    }
}