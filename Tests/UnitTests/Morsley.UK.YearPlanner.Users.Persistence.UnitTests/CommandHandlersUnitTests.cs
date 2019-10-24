//using AutoFixture;
//using Morsley.UK.YearPlanner.Users.Application.Commands;
//using Morsley.UK.YearPlanner.Users.Application.Handlers;
//using NSubstitute;
//using System.Threading.Tasks;
//using Morsley.UK.YearPlanner.Users.Domain.Interfaces;
//using Xunit;

//namespace Morsley.UK.YearPlanner.Users.Persistence.UnitTests
//{
//    public class CommandHandlersUnitTests
//    {
//        private Fixture _fixture;

//        public CommandHandlersUnitTests()
//        {
//            _fixture = new Fixture();
//        }

//        [Fact]
//        public async Task AddUserHandler_Should_Add_User()
//        {
//            // Arrange...
//            var mockUnitOfWork = Substitute.For<IUnitOfWork>();
//            mockUnitOfWork.CompleteAsync().Returns(1);
//            var sut = new AddUserCommandHandler(mockUnitOfWork);
//            var addUserCommand = _fixture.Create<AddUserCommand>();

//            // Act...
//            await sut.Handle(addUserCommand);

//            // Assert...
//            // ???
//        }
//    }
//}