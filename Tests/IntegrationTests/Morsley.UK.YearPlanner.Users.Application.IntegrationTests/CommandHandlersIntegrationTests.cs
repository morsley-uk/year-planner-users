using AutoFixture;
using FluentAssertions;
using Morsley.UK.YearPlanner.Users.Application.Commands;
using Morsley.UK.YearPlanner.Users.Application.Handlers;
using Morsley.UK.YearPlanner.Users.Domain.Enumerations;
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
    public class CommandHandlersIntegrationTests
    {
        private readonly Fixture _fixture;

        public CommandHandlersIntegrationTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public async Task AddUserHandler_Should_Add_User()
        {
            // Arrange...
            var mockUnitOfWork = GetMockUnitOfWork(out var inMemoryContext);
            var sut = new AddUserCommandHandler(mockUnitOfWork);
            var addUserCommand = _fixture.Create<AddUserCommand>();
            var ct = new CancellationToken();

            // Act...
            await sut.Handle(addUserCommand, ct);

            // Assert...
            inMemoryContext.Users.Count().Should().Be(1);
        }

        [Fact]
        public async Task DeleteUserHandler_Should_Delete_User()
        {
            // Arrange...
            var mockUnitOfWork = GetMockUnitOfWork(out var inMemoryContext);
            var user = _fixture.Create<User>();
            InMemoryContextHelper.AddUserToContext(inMemoryContext, user);
            var sut = new DeleteUserCommandHandler(mockUnitOfWork);
            var deleteUserCommand = new DeleteUserCommand
            {
                Id = user.Id
            };
            var ct = new CancellationToken();

            // Act...
            await sut.Handle(deleteUserCommand, ct);

            // Assert...
            inMemoryContext.Users.Count().Should().Be(0);
        }

        [Fact]
        public async Task UpdateUserHandler_Should_Update_User()
        {
            // Arrange...
            var mockUnitOfWork = GetMockUnitOfWork(out var inMemoryContext);
            var user = _fixture.Create<User>();
            InMemoryContextHelper.AddUserToContext(inMemoryContext, user);
            var sut = new UpdateUserCommandHandler(mockUnitOfWork);
            var updateUserCommand = new UpdateUserCommand
            {
                Id = user.Id,
                Title = _fixture.Create<Title>(),
                FirstName = _fixture.Create<string>(),
                LastName = _fixture.Create<string>(),
                Sex = _fixture.Create<Sex>()
            };
            var ct = new CancellationToken();

            // Act...
            await sut.Handle(updateUserCommand, ct);

            // Assert...
            inMemoryContext.Users.Count().Should().Be(1);
            user.Title.Should().Be(updateUserCommand.Title);
            user.FirstName.Should().Be(updateUserCommand.FirstName);
            user.LastName.Should().Be(updateUserCommand.LastName);
            user.Sex.Should().Be(updateUserCommand.Sex);
        }

        #region Helper Methods

        private IUnitOfWork GetMockUnitOfWork(out DataContext inMemoryContext)
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