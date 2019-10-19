using Microsoft.EntityFrameworkCore;
using Morsley.UK.YearPlanner.Users.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Morsley.UK.YearPlanner.Users.Persistence.Models
{
    public class PagedList<T> : List<T>, IPagedList<T>
    {
        protected PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            AddRange(items);
            TotalCount = count;
            CurrentPage = pageNumber;
            PageSize = pageSize;
            if (count == 0)
            {
                TotalPages = 0;
            }
            else
            {
                TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            }
        }

        public int CurrentPage { get; private set; }

        public int TotalPages { get; private set; }

        public int PageSize { get; private set; }

        public int TotalCount { get; private set; }

        public bool HasPrevious => CurrentPage > 1;

        public bool HasNext => CurrentPage < TotalPages;

        public static async Task<PagedList<T>> Create(IQueryable<T> source, int pageNumber, int pageSize)
        {
            if (source == null) {throw new ArgumentNullException(nameof(source), "Cannot be null!");}
            if (pageNumber == 0) throw new ArgumentOutOfRangeException(nameof(pageNumber), "Must be greater than zero!");
            if (pageSize == 0) throw new ArgumentOutOfRangeException(nameof(pageSize), "Must be greater than zero!");

            var count = source.Count();
            if (count == 0)
            {
                return new PagedList<T>(source, 0, 0, 0);
            }
            else
            {
                var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
                return new PagedList<T>(items, count, pageNumber, pageSize);
            }
        }
    }
}