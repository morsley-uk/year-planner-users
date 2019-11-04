using MediatR;
using Morsley.UK.YearPlanner.Users.Application.Commands;
using Morsley.UK.YearPlanner.Users.Domain.Interfaces;
using Morsley.UK.YearPlanner.Users.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Morsley.UK.YearPlanner.Users.Application.Handlers
{
    public sealed class PartiallyUpdateUserCommandHandler : IRequestHandler<PartiallyUpdateUserCommand, User>
    {
        private readonly IUnitOfWork _unitOfWork;

        public PartiallyUpdateUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> Handle(PartiallyUpdateUserCommand command, CancellationToken ct)
        {
            var user = await _unitOfWork.UserRepository.Get(command.Id);
            if (user == null) throw new ArgumentException("User not found!", nameof(command));

            UpdateUserFromCommand(user, command);

            _unitOfWork.UserRepository.Update(user);
            var numberOfRowsAffected = await _unitOfWork.CompleteAsync();
            // ToDo --> Log!

            return user;
        }

        private void UpdateUserFromCommand(User user, PartiallyUpdateUserCommand command)
        {
            if (command.TitleChanged && user.Title != command.Title) user.Title = command.Title;
            if (command.FirstNameChanged && user.FirstName != command.FirstName) user.FirstName = command.FirstName;
            if (command.LastNameChanged && user.LastName != command.LastName) user.LastName = command.LastName;
            if (command.SexChanged && user.Sex != command.Sex) user.Sex = command.Sex;
        }
    }
}