using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Morsley.UK.YearPlanner.Users.Domain.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class, IEntity<Guid>
    {
        void Create(TEntity entity);

        void Delete(TEntity entity);

        void Delete(Guid id);

        Task<bool> Exists(Guid id);

        IEnumerable<TEntity> Find(Func<TEntity, bool> predicate);

        Task<TEntity> Get(Guid id);

        Task<IPagedList<TEntity>> Get(IGetOptions options);

        void Update(TEntity entity);
    }
}