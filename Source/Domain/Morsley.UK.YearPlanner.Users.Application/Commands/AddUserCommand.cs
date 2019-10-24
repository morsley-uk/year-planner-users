using MediatR;
using Morsley.UK.YearPlanner.Users.Domain.Enumerations;
using System;

namespace Morsley.UK.YearPlanner.Users.Application.Commands
{
    public sealed class AddUserCommand : IRequest<Guid>
    {
        public Title Title { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Sex Sex { get; set; }
    }
}