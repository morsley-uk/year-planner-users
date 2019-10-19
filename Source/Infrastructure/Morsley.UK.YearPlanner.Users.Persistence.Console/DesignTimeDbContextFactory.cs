using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Morsley.UK.YearPlanner.Users.Persistence.Contexts;

namespace Morsley.UK.YearPlanner.Users.Persistence.Console
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        DataContext IDesignTimeDbContextFactory<DataContext>.CreateDbContext(string[] args)
        {
            var connectionString = ConfigurationHelper.GetConnectionString();

            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new DataContext(optionsBuilder.Options);
        }
    }
}