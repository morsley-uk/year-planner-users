using Morsley.UK.YearPlanner.Users.API.Models.v1.Response;
using Morsley.UK.YearPlanner.Users.Domain.Interfaces;
using Morsley.UK.YearPlanner.Users.Domain.Models;
using System.Collections.Generic;

namespace Morsley.UK.YearPlanner.Users.API.Models.v1
{
    public class PagedList : List<UserResponse>, IPagedList<UserResponse>
    {
        protected PagedList() { }

        protected PagedList(IPagedList<UserResponse> pageOfUsers)
        {
            AddRange(pageOfUsers);
            TotalCount = pageOfUsers.Count;
            CurrentPage = pageOfUsers.CurrentPage;
            PageSize = pageOfUsers.PageSize; 
            TotalPages = pageOfUsers.TotalPages;
        }

        private static IPagedList<UserResponse> PageOfUserResponsesFromPageOfUsers(IEnumerable<User> pageOfUsers)
        {
            var userResponses = new PagedList();
            foreach (var user in pageOfUsers)
            {
                var userResponse = UserResponseFromUser(user);
                
                userResponses.Add(userResponse);
            }
            return userResponses;
        }

        private static UserResponse UserResponseFromUser(User user)
        {
            var userResponse = new UserResponse();

            // ToDo --> Use AutoMapper!

            return userResponse;
        }

        public int CurrentPage { get; private set; }

        public int TotalPages { get; private set; }

        public int PageSize { get; private set; }

        public int TotalCount { get; private set; }

        public bool HasPrevious => CurrentPage > 1;

        public bool HasNext => CurrentPage < TotalPages;

        public static PagedList Create(IPagedList<User> pageOfUsers)
        {
            var pageOfUserResponses = PageOfUserResponsesFromPageOfUsers(pageOfUsers);

            return new PagedList(pageOfUserResponses);
        }
    }
}