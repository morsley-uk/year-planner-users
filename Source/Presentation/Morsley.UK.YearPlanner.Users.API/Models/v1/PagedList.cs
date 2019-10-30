using Morsley.UK.YearPlanner.Users.Domain.Interfaces;
using System.Collections.Generic;

namespace Morsley.UK.YearPlanner.Users.API.Models.v1
{
    public class PagedList<T> : List<T>, IPagedList<T>
    {
        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public bool HasPrevious => CurrentPage > 1;

        public bool HasNext => CurrentPage < TotalPages;
    }
}