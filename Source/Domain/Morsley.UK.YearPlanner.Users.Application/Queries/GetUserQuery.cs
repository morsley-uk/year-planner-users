using MediatR;
using Morsley.UK.YearPlanner.Users.Domain.Models;
using System;

namespace Morsley.UK.YearPlanner.Users.Application.Queries
{
    public sealed class GetUserQuery : IRequest<User>
    {
        public Guid Id { get; set; }
    }
}