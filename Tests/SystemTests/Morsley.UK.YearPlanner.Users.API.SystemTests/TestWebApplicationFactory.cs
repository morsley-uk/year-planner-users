using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Morsley.UK.YearPlanner.Users.Domain.Interfaces;
using Morsley.UK.YearPlanner.Users.Persistence.Contexts;
using System.Linq;

namespace Morsley.UK.YearPlanner.Users.API.SystemTests
{
    public class TestWebApplicationFactory<TStartUp> : WebApplicationFactory<TStartUp> where TStartUp : class
    {
        //protected override IHostBuilder CreateHostBuilder()
        //{
        //    var builder = base.CreateHostBuilder();
        //    //.ConfigureHostConfiguration(configure => configure.AddEnvironmentVariables(""));

        //    builder.UseEnvironment("Testing");

        //    return builder;
        //}

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                AddInMemoryDatabase(services);
                AddTestEnvironmentService(services);
                //EnsureDatabaseIsCreated(services);

                //try
                //{
                //    // Seed the database with test data.
                //    //Utilities.InitializeDbForTests(db);
                //}
                //catch (Exception e)
                //{
                //    logger.LogError(
                //        e,
                //        "An error occurred seeding the database with test messages. Error: {Message}",
                //        e.Message);
                //}
            });
        }

        private static void EnsureDatabaseIsCreated(IServiceCollection services)
        {
            // Build the service provider.
            var sp = services.BuildServiceProvider();

            // Create a scope to obtain a reference to the database context (DataContext).
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var context = scopedServices.GetRequiredService<DataContext>();
            var logger = scopedServices.GetRequiredService<ILogger<TestWebApplicationFactory<TStartUp>>>();

            // Ensure the database is created.
            context.Database.EnsureCreated();
        }

        private void AddTestEnvironmentService(IServiceCollection services)
        {
            // Remove the application's EnvironmentService registration.
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(Infrastructure.EnvironmentService));
            if (descriptor != null) services.Remove(descriptor);

            services.AddScoped<IEnvironmentService, TestEnvironmentService>();
        }

        private static void AddInMemoryDatabase(IServiceCollection services)
        {
            // Remove the application's DataContext registration.
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DataContext>));
            if (descriptor != null) services.Remove(descriptor);

            // Add DataContext using an in-memory database for testing.
            services.AddDbContext<DataContext>(context => context.UseInMemoryDatabase("InMemoryDatabaseForTesting"));
        }
    }
}