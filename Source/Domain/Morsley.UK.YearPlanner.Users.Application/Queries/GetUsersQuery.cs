using MediatR;
using Morsley.UK.YearPlanner.Users.Domain.Interfaces;
using Morsley.UK.YearPlanner.Users.Domain.Models;

namespace Morsley.UK.YearPlanner.Users.Application.Queries
{
    public sealed class GetUsersQuery : IRequest<IPagedList<User>>
    {
        public uint PageNumber { get; set; }

        public uint PageSize { get; set; }

        public string SearchQuery { get; set; }

    }
}