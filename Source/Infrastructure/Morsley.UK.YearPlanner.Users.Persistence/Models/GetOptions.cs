//using Morsley.UK.YearPlanner.Users.Domain.Interfaces;
//using System;
//using System.Collections.Generic;

//namespace Morsley.UK.YearPlanner.Users.Persistence.Models
//{
//    public class GetOptions : IGetOptions
//    {
//        private readonly IList<IFilter> _filters;
//        private readonly IList<IOrdering> _orderings;

//        public GetOptions()
//        {
//            _filters = new List<IFilter>();
//            _orderings = new List<IOrdering>();
//        }

//        public int PageSize { get; set; } = 25;

//        public int PageNumber { get; set; } = 1;

//        public string SearchQuery { get; set; }

//        public IEnumerable<IFilter> Filters => _filters;

//        public IEnumerable<IOrdering> Orderings => _orderings;

//        public void AddFilter(IFilter filter)
//        {
//            if (filter == null) throw new ArgumentNullException(nameof(filter));

//            if (!CanAddFilter(filter)) throw new ArgumentException("Duplicate!", nameof(filter));
//            _filters.Add(filter);
//        }

//        public void AddOrdering(IOrdering ordering)
//        {
//            if (ordering == null) throw new ArgumentNullException(nameof(ordering));

//            if (!CanAddOrdering(ordering)) throw new ArgumentException("Duplicate!", nameof(ordering));
//            _orderings.Add(ordering);
//        }

//        private bool CanAddFilter(IFilter filter)
//        {
//            if (filter == null) throw new ArgumentNullException(nameof(filter));

//            if (_filters.Contains(filter)) return false;

//            return true;
//        }

//        private bool CanAddOrdering(IOrdering ordering)
//        {
//            if (ordering == null) throw new ArgumentNullException(nameof(ordering));

//            if (_orderings.Contains(ordering)) return false;

//            return true;
//        }
//    }
//}