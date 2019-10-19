using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Morsley.UK.YearPlanner.Users.Persistence.Contexts;
using Morsley.UK.YearPlanner.Users.Shared;

namespace Morsley.UK.YearPlanner.Users.Persistence.Console
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            System.Console.WriteLine("Expect the Data Connection String to be in an environment variable: {0}", Shared.Constants.EnvironmentVariables.UsersPersistenceKey);

            if (!TryGetConnectionString(out var connectionString))
            {
                System.Console.Error.WriteLine("Could not determine the Data Connection String");
            }

            SetUpDependencyInjection(connectionString);

            System.Console.WriteLine("Hello World!");
        }

        private static void SetUpDependencyInjection(string connectionString)
        {
            var serviceProvider = new ServiceCollection()
                .AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));
        }

        private static bool TryGetConnectionString(out string connectionString)
        {
            try
            {
                connectionString = Shared.Environment.GetEnvironmentVariableValueByKey(Shared.Constants.EnvironmentVariables.UsersPersistenceKey);
                return connectionString != null;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                connectionString = string.Empty;
                return false;
            }
        }
    }
}