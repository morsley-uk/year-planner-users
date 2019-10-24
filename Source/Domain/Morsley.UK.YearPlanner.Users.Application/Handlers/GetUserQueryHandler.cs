using MediatR;
using Morsley.UK.YearPlanner.Users.Application.Queries;
using Morsley.UK.YearPlanner.Users.Domain.Interfaces;
using Morsley.UK.YearPlanner.Users.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Morsley.UK.YearPlanner.Users.Application.Handlers
{
    public sealed class GetUserQueryHandler : IRequestHandler<GetUserQuery, User>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> Handle(GetUserQuery query, CancellationToken ct)
        {
            var user = await _unitOfWork.UserRepository.Get(query.Id);
            return user;
        }
    }
}