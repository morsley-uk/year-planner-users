using Microsoft.Extensions.DependencyInjection;
using Morsley.UK.YearPlanner.Users.Domain.Interfaces;

namespace Morsley.UK.YearPlanner.Users.Infrastructure.IoC
{
    public static class InfrastructureServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IDateTimeService, DateTimeService>();
            services.AddScoped<IEnvironmentService, EnvironmentService>();

            return services;
        }
    }
}