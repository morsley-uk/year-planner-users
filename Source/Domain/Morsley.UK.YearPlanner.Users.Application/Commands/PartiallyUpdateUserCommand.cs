using MediatR;
using Morsley.UK.YearPlanner.Users.Domain.Enumerations;
using Morsley.UK.YearPlanner.Users.Domain.Models;
using System;

namespace Morsley.UK.YearPlanner.Users.Application.Commands
{
    public sealed class PartiallyUpdateUserCommand : IRequest<User>
    {
        public Guid Id { get; set; }

        public Title? Title { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public Sex? Sex { get; set; }

        public bool TitleChanged { get; set; }

        public bool FirstNameChanged { get; set; }

        public bool LastNameChanged { get; set; }

        public bool SexChanged { get; set; }
    }
}