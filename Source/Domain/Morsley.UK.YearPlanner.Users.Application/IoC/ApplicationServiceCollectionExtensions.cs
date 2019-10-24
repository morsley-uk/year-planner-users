using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Morsley.UK.YearPlanner.Users.Application.IoC
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {

            var executingAssembly = Assembly.GetExecutingAssembly();
            services.AddMediatR(executingAssembly);

            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            return services;
        }
    }
}