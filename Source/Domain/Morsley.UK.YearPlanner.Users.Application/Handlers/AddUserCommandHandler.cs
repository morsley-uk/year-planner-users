using MediatR;
using Morsley.UK.YearPlanner.Users.Application.Commands;
using Morsley.UK.YearPlanner.Users.Domain.Interfaces;
using Morsley.UK.YearPlanner.Users.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Morsley.UK.YearPlanner.Users.Application.Handlers
{
    public sealed class AddUserCommandHandler : IRequestHandler<AddUserCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(AddUserCommand command, CancellationToken ct)
        {
            var user = new User(command.FirstName, command.LastName);

            _unitOfWork.UserRepository.Create(user);

            var numberOfRowsAffected = await _unitOfWork.CompleteAsync();
            // ToDo --> Log!

            return user.Id;
        }
    }
}