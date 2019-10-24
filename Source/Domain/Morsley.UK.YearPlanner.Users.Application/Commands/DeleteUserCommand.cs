using MediatR;
using System;

namespace Morsley.UK.YearPlanner.Users.Application.Commands
{
    public class DeleteUserCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}