using MediatR;
using Morsley.UK.YearPlanner.Users.Application.Queries;
using Morsley.UK.YearPlanner.Users.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Morsley.UK.YearPlanner.Users.Application.Handlers
{
    public sealed class UserExistsQueryHandler : IRequestHandler<UserExistsQuery, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserExistsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UserExistsQuery query, CancellationToken ct)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            return await _unitOfWork.UserRepository.Exists(query.Id);
        }
    }
}