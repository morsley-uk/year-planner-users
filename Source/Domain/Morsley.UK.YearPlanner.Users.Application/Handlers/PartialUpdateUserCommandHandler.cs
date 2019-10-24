using MediatR;
using Morsley.UK.YearPlanner.Users.Application.Commands;
using Morsley.UK.YearPlanner.Users.Domain.Interfaces;
using Morsley.UK.YearPlanner.Users.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Morsley.UK.YearPlanner.Users.Application.Handlers
{
    public sealed class PartialUpdateUserCommandHandler : IRequestHandler<PartialUpdateUserCommand, User>
    {
        private readonly IUnitOfWork _unitOfWork;

        public PartialUpdateUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> Handle(PartialUpdateUserCommand command, CancellationToken ct)
        {
            var user = await _unitOfWork.UserRepository.Get(command.Id);
            if (user == null) throw new ArgumentException("User not found!", nameof(command));

            UpdateUserFromCommand(user, command);

            var numberOfRowsAffected = await _unitOfWork.CompleteAsync();
            // ToDo --> Log!

            return user;
        }

        private void UpdateUserFromCommand(User user, PartialUpdateUserCommand command)
        {
            if (user.Title != command.Title) user.Title = command.Title;
            if (user.FirstName != command.FirstName) user.SetFirstName(command.FirstName);
            if (user.FirstName != command.FirstName) user.SetFirstName(command.FirstName);
            if (user.Sex != command.Sex) user.Sex = command.Sex;
        }
    }
}