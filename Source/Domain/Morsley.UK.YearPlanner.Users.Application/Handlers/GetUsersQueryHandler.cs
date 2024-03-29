﻿using AutoMapper;
using MediatR;
using Morsley.UK.YearPlanner.Users.Application.Models;
using Morsley.UK.YearPlanner.Users.Application.Queries;
using Morsley.UK.YearPlanner.Users.Domain.Interfaces;
using Morsley.UK.YearPlanner.Users.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Morsley.UK.YearPlanner.Users.Application.Handlers
{
    public sealed class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IPagedList<User>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IPagedList<User>> Handle(GetUsersQuery query, CancellationToken ct)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            var getOptions = _mapper.Map<GetOptions>(query);
            var pageOfUsers = await _unitOfWork.UserRepository.Get(getOptions);
            return pageOfUsers;
        }
    }
}