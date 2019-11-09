using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Morsley.UK.YearPlanner.Users.Domain.Interfaces;
using Morsley.UK.YearPlanner.Users.Persistence.Contexts;
using Morsley.UK.YearPlanner.Users.Persistence.Repositories;
using System;

namespace Morsley.UK.YearPlanner.Users.Persistence.IoC
{
    public static class PersistenceSqlServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            var sp = services.BuildServiceProvider();

            var environmentService = sp.GetService<IEnvironmentService>();

            var settings = environmentService.GetVariable(Shared.Constants.EnvironmentVariables.UsersPersistenceKey);

            services.AddDbContext<DataContext>((provider, options) =>
            {
                options.ConfigureWarnings(builder => builder.Default(WarningBehavior.Log))
                       .UseSqlServer(settings, sqlOptions => sqlOptions.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30), null));
            });

            // ToDo --> Should the following go in Main (Program.cs)?

            //var context = sp.GetService<DataContext>();
            //try
            //{
            //    Log.Debug("Trying to ensure the database is created...");
            //    context.Database.EnsureCreated();
            //    Log.Debug("Database either existed or was now created.");
            //}
            //catch (Exception e)
            //{
            //    Log.Fatal(e, "Cannot ensure database created!");
            //}

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserRepository, UserRepository>();
            
            return services;
        }
    }
}