using AutoMapper;
using MediatR;
using Morsley.UK.YearPlanner.Users.Application.Models;
using Morsley.UK.YearPlanner.Users.Application.Queries;
using Morsley.UK.YearPlanner.Users.Domain.Interfaces;
using Morsley.UK.YearPlanner.Users.Domain.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Morsley.UK.YearPlanner.Users.Application.Handlers
{
    public sealed class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IPagedList<User>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUsersQueryHandler(IUnitOfWork unitOfWork) //, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            //_mapper = mapper;
        }

        public async Task<IPagedList<User>> Handle(GetUsersQuery query, CancellationToken ct)
        {
            var getOptions = GetOptionsFromQuery(query);
            var pageOfUsers = await _unitOfWork.UserRepository.Get(getOptions);
            return pageOfUsers;
        }

        private GetOptions GetOptionsFromQuery(GetUsersQuery query)
        {
            // ToDo --> Use AutoMapper!
            var getOptions = new GetOptions();



            return getOptions;
        }
    }
}