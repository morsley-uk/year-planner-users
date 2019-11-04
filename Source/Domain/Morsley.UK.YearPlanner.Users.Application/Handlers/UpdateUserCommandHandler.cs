using MediatR;
using Morsley.UK.YearPlanner.Users.Application.Commands;
using Morsley.UK.YearPlanner.Users.Domain.Interfaces;
using Morsley.UK.YearPlanner.Users.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Morsley.UK.YearPlanner.Users.Application.Handlers
{
    public sealed class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, User>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserCommandHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> Handle(
            UpdateUserCommand command,
            CancellationToken ct)
        {
            Domain.Models.User user = await _unitOfWork.UserRepository.Get(command.Id);
            if (user == null) throw new ArgumentException("User not found!", nameof(command));
            return await UpdateUser(user, command, ct);
        }

        private async Task<User> UpdateUser(
            User user,
            UpdateUserCommand command,
            CancellationToken ct)
        {
            if (user.Title != command.Title) user.Title = command.Title;
            if (user.FirstName != command.FirstName) user.FirstName = command.FirstName;
            if (user.LastName != command.LastName) user.LastName = command.LastName;
            if (user.Sex != command.Sex) user.Sex = command.Sex;

            _unitOfWork.UserRepository.Update(user);
            var numberOfRowsAffected = await _unitOfWork.CompleteAsync();
            // ToDo --> Log!

            return user;
        }
    }
}