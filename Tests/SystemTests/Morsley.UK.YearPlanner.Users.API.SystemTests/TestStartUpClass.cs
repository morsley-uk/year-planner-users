using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Morsley.UK.YearPlanner.Users.API.SystemTests
{
    public class TestStartUp : API.StartUp
    {
        public TestStartUp(IConfiguration configuration) : base(configuration)
        {
        }

        protected override void AddPersistence(IServiceCollection services)
        {
            //base.AddPersistence(services);
        }
    }
}