using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Morsley.UK.YearPlanner.Users.Application.Commands;
using Morsley.UK.YearPlanner.Users.Application.Handlers;
using Morsley.UK.YearPlanner.Users.Domain.Interfaces;
using Morsley.UK.YearPlanner.Users.Domain.Models;
using NSubstitute;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Morsley.UK.YearPlanner.Users.Persistence.UnitTests
{
    public class CommandHandlersUnitTests
    {
        private Fixture _fixture;

        public CommandHandlersUnitTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public async Task AddUserHandler_Should_Add_User()
        {
            // Arrange...
            Domain.Models.User addedUser = _fixture.Create<Domain.Models.User>();
            var mockUnitOfWork = Substitute.For<IUnitOfWork>();
            //var expectedUserId = _fixture.Create<Guid>();
            //mockUnitOfWork.UserRepository.When(x => x.Create(Arg.Any<User>()))
            //                             .Do(x => x.Arg<User>().Id = expectedUserId);
            //var expectedUserId = _fixture.Create<Guid>();

            mockUnitOfWork.UserRepository.When(x => x.Create(Arg.Any<User>()))
                                         .Do(x => x.Arg<User>().Id = addedUser.Id);
            mockUnitOfWork.CompleteAsync().Returns(1);
            var mockMapper = Substitute.For<IMapper>();
            //Domain.Models.User addedUser = _fixture.Create<Domain.Models.User>();
            mockMapper.Map<Domain.Models.User>(Arg.Any<Application.Commands.AddUserCommand>()).Returns(addedUser);

            var sut = new AddUserCommandHandler(mockUnitOfWork, mockMapper);
            var addUserCommand = _fixture.Create<AddUserCommand>();
            var ct = new CancellationToken();

            // Act...
            var actualUserId = await sut.Handle(addUserCommand, ct);

            // Assert...
            //actualUserId.Should().Be(expectedUserId);
            actualUserId.Should().Be(addedUser.Id);
            mockUnitOfWork.UserRepository.Received(1).Create(Arg.Any<User>());
            await mockUnitOfWork.Received(1).CompleteAsync();
        }

        [Fact]
        public void AddUserHandler_Should_Throw_When_Command_Is_Null()
        {
            // Arrange...
            var mockUnitOfWork = Substitute.For<IUnitOfWork>();
            var mockMapper = Substitute.For<IMapper>();

            var sut = new AddUserCommandHandler(mockUnitOfWork, mockMapper);
            
            const AddUserCommand addUserCommand = null;
            var ct = new CancellationToken();

            // Act...
            Func<Task> action = async () => await sut.Handle(addUserCommand, ct);

            // Assert...
            action.Should().Throw<ArgumentNullException>();

        }

        // ToDo --> Test CancellationToken
    }
}