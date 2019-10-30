using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Morsley.UK.YearPlanner.Users.Application.Commands;
using Morsley.UK.YearPlanner.Users.Application.Models;
using Morsley.UK.YearPlanner.Users.Application.Profiles;
using Morsley.UK.YearPlanner.Users.Application.Queries;
using Morsley.UK.YearPlanner.Users.Domain.Models;
using Xunit;

namespace Morsley.UK.YearPlanner.Application.UnitTests
{
    public class AutoMapperUnitTests
    {
        private readonly Fixture _fixture;

        public AutoMapperUnitTests()
        {
            _fixture = new Fixture();
            //_fixture.Customizations.Insert(0, new EnumSpecimenBuilder<Title>());
            //_fixture.Customizations.Insert(1, new EnumSpecimenBuilder<Sex>());
            //_fixture.Customizations.Add(new CreateUserRequestSpecimenBuilder());
            //_fixture.Customizations.Add(new DomainUserSpecimenBuilder());
        }

        [Fact]
        public void AddUserCommand_To_User()
        {
            // Arrange...
            var configuration = new MapperConfiguration(configure =>
            {
                configure.AddProfile<AddUserCommandToUser>();
            });
            var sut = configuration.CreateMapper();
            var addUserCommand = _fixture.Create<AddUserCommand>();

            // Act...
            var user = sut.Map<User>(addUserCommand);

            // Assert...
            user.Should().BeEquivalentTo(addUserCommand);
        }

        [Fact]
        public void AddUserCommand_To_User_With_Default_Values()
        {
            // Arrange...
            var configuration = new MapperConfiguration(configure =>
            {
                configure.AddProfile<AddUserCommandToUser>();
            });
            var sut = configuration.CreateMapper();
            var firstName = _fixture.Create<string>();
            var lastName = _fixture.Create<string>();
            var addUserCommand = new AddUserCommand
            {
                Title = null,
                FirstName = firstName,
                LastName = lastName,
                Sex = null
            };

            // Act...
            var user = sut.Map<User>(addUserCommand);

            // Assert...
            user.Should().BeEquivalentTo(addUserCommand);
        }

        [Fact]
        public void GetUsersQuery_To_GetOptions()
        {
            // Arrange...
            var configuration = new MapperConfiguration(configure =>
            {
                configure.AddProfile<GetUsersQueryToGetOptions>();
            });
            var sut = configuration.CreateMapper();
            var getUsersQuery = _fixture.Create<GetUsersQuery>();

            // Act...
            var getOptions = sut.Map<GetOptions>(getUsersQuery);

            // Assert...
            getOptions.Should().BeEquivalentTo(getUsersQuery);
        }

        [Fact]

        public void GetUsersQuery_To_GetOptions_With_Default_Values()
        {
            // Arrange...
            var configuration = new MapperConfiguration(configure =>
            {
                configure.AddProfile<GetUsersQueryToGetOptions>();
            });
            var sut = configuration.CreateMapper();
            var getUsersQuery = new GetUsersQuery();

            // Act...
            var getOptions = sut.Map<GetOptions>(getUsersQuery);

            // Assert...
            getOptions.Should().BeEquivalentTo(getUsersQuery);
        }
    }
}