using Microsoft.EntityFrameworkCore;
using Morsley.UK.YearPlanner.Users.Domain.Enumerations;
using Morsley.UK.YearPlanner.Users.Domain.Interfaces;
using Morsley.UK.YearPlanner.Users.Persistence.Contexts;
using Morsley.UK.YearPlanner.Users.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Morsley.UK.YearPlanner.Users.Persistence.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity<Guid>
    {
        protected readonly DataContext Context;

        protected Repository(DataContext context)
        {
            Context = context;
        }

        public void Create(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }

        public async Task<TEntity> Get(Guid id)
        {
            return await Context.Set<TEntity>()
                                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public virtual async Task<IPagedList<TEntity>> Get(IGetOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            var entities = GetAll(options);

            return await PagedList<TEntity>.CreateAsync(entities, options.PageNumber, options.PageSize);
        }

        protected virtual IQueryable<TEntity> Search(IQueryable<TEntity> entities, IGetOptions options)
        {
            return entities;
        }

        protected virtual IQueryable<TEntity> Sort(IQueryable<TEntity> entities, IGetOptions options)
        {
            if (!options.Orderings.Any()) return entities;

            return entities.OrderBy(ToOrderByString(options.Orderings));
        }

        private string ToOrderByString(IEnumerable<IOrdering> orderings)
        {
            var orderBys = new List<string>();

            foreach (var ordering in orderings)
            {
                var orderBy = ordering.Key;
                switch (ordering.Order)
                {
                    case SortOrder.Ascending:
                        orderBy += " asc";
                        break;
                    case SortOrder.Descending:
                        orderBy += " desc";
                        break;
                }
                orderBys.Add(orderBy);
            }

            return string.Join(",", orderBys);
        }

        protected virtual IQueryable<TEntity> GetAll(IGetOptions options)
        {
            var entities = Context.Set<TEntity>().AsQueryable();

            entities = Sort(entities, options);
            entities = Filter(entities, options);
            entities = Search(entities, options);

            return entities;
        }

        protected virtual IQueryable<TEntity> Filter(IQueryable<TEntity> entities, IGetOptions options)
        {
            return entities;
        }

        public IEnumerable<TEntity> Find(Func<TEntity, bool> predicate)
        {
            return Context.Set<TEntity>().Where(predicate).ToList();
        }

        public void Delete(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public void Delete(Guid id)
        {
            var entityToDelete = Context.Set<TEntity>().FirstOrDefault(e => e.Id == id);
            if (entityToDelete != null)
            {
                Context.Set<TEntity>().Remove(entityToDelete);
            }
        }
    }
}