using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Morsley.UK.YearPlanner.Users.Application.Handlers;
using Morsley.UK.YearPlanner.Users.Application.Profiles;
using Morsley.UK.YearPlanner.Users.Application.Queries;
using Morsley.UK.YearPlanner.Users.Domain.Interfaces;
using Morsley.UK.YearPlanner.Users.Domain.Models;
using Morsley.UK.YearPlanner.Users.Persistence.Contexts;
using Morsley.UK.YearPlanner.Users.Persistence.Repositories;
using Morsley.UK.YearPlanner.Users.Tests.Shared;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Morsley.UK.YearPlanner.Users.Application.IntegrationTests
{
    public class QueryHandlersIntegrationTests
    {
        private readonly Fixture _fixture;

        public QueryHandlersIntegrationTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public async Task GetUserHandler_Should_Get_User()
        {
            // Arrange...
            var unitOfWork = GetUnitOfWork(out var inMemoryContext);
            var user = _fixture.Create<User>();
            InMemoryContextHelper.AddUserToContext(inMemoryContext, user);
            var sut = new GetUserQueryHandler(unitOfWork);
            var getUserQuery = new GetUserQuery
            {
                Id = user.Id
            };
            var ct = new CancellationToken();

            // Act...
            var result = await sut.Handle(getUserQuery, ct);

            // Assert...
            result.Should().Be(user);
        }

        [Fact]
        public async Task GetUsersHandler_Should_Get_Users()
        {
            // Arrange...
            var unitOfWork = GetUnitOfWork(out var inMemoryContext);
            InMemoryContextHelper.AddUsersToContext(_fixture, inMemoryContext, 5);
            var mapper = GetMapper();
            var sut = new GetUsersQueryHandler(unitOfWork, mapper);
            var getUsersQuery = new GetUsersQuery
            {
                PageNumber = 1,
                PageSize = 10
            };
            var ct = new CancellationToken();

            // Act...
            await sut.Handle(getUsersQuery, ct);

            // Assert...
            inMemoryContext.Users.Count().Should().Be(5);
        }

        #region Helper Methods

        private IMapper GetMapper()
        {
            var mapperConfiguration = new MapperConfiguration(configure =>
            {
                configure.AddProfile<GetUsersQueryToGetOptions>();
            });
            return mapperConfiguration.CreateMapper();
        }

        private IUnitOfWork GetUnitOfWork(out DataContext inMemoryContext)
        {
            inMemoryContext = InMemoryContextHelper.Create();
            var userRepository = new UserRepository(inMemoryContext);
            var dt = _fixture.Create<DateTime>();
            var mockDateTimeService = MockDateTimeServiceHelper.Create(dt);
            var unitOfWork = new Persistence.UnitOfWork(inMemoryContext, mockDateTimeService, userRepository);
            return unitOfWork;
        }

        #endregion Helper Methods
    }
}