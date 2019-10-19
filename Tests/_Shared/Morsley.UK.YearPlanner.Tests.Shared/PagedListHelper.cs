using Morsley.UK.YearPlanner.Users.Domain.Interfaces;
using Morsley.UK.YearPlanner.Users.Persistence.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Morsley.UK.YearPlanner.Users.Tests.Shared
{
    public static class PagedListHelper<T> where T : class
    {
        public static async Task<IPagedList<T>> Create(IQueryable<T> entities, int pageNumber, int pageSize)
        {
            var pagedList = await PagedList<T>.Create(entities, pageNumber, pageSize);
            return pagedList;
        }
    }
}