using System;
using System.Threading.Tasks;

namespace Morsley.UK.YearPlanner.Users.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }

        Task<int> CompleteAsync();
    }
}