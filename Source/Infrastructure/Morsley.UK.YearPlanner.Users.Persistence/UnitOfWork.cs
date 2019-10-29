using Microsoft.EntityFrameworkCore;
using Morsley.UK.YearPlanner.Users.Domain.Interfaces;
using Morsley.UK.YearPlanner.Users.Persistence.Contexts;
using System;
using System.Threading.Tasks;

namespace Morsley.UK.YearPlanner.Users.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly DataContext Context;
        protected readonly IDateTimeService DateTimeService;

        public UnitOfWork(
            DataContext context,
            IDateTimeService _dateTimeService,
            IUserRepository userRepository)
        {
            Context = context;
            DateTimeService = _dateTimeService;
            UserRepository = userRepository;
        }

        public IUserRepository UserRepository { get; }

        public async Task<int> CompleteAsync()
        {
            var entries = Context.ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                var entity = entry.Entity as IEntity<Guid>;
                if (entity is null) continue;
                if (entry.State == EntityState.Added)
                {
                    entity.Created = DateTimeService.GetDateTimeUtcNow();
                }
                else if (entry.State == EntityState.Modified)
                {
                    entity.Updated = DateTimeService.GetDateTimeUtcNow();
                }
            }
            //await UserRepository.CompleteAsync();
            return await Context.SaveChangesAsync();
            //return 0;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;

            Context?.Dispose();
        }
    }
}