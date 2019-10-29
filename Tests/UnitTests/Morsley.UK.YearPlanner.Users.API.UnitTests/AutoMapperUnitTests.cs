using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Morsley.UK.YearPlanner.Users.API.Models.v1.Request;
using Morsley.UK.YearPlanner.Users.API.Profiles;
using Morsley.UK.YearPlanner.Users.API.UnitTests.AutoFixture;
using Morsley.UK.YearPlanner.Users.Application.Commands;
using Morsley.UK.YearPlanner.Users.Domain.Enumerations;
using Morsley.UK.YearPlanner.Users.Tests.Shared.AutoFixture;
using System;
using Xunit;

namespace Morsley.UK.YearPlanner.Users.API.UnitTests
{
    public class AutoMapperUnitTests
    {
        private Fixture _fixture;


        public AutoMapperUnitTests()
        {
            _fixture = new Fixture();
            _fixture.Customizations.Insert(0, new EnumSpecimenBuilder<Title>());
            _fixture.Customizations.Insert(1, new EnumSpecimenBuilder<Sex>());
            _fixture.Customizations.Add(new CreateUserRequestSpecimenBuilder());
        }

        [Fact]
        public void CreateUserRequest_To_AddUserCommand_When_Sex_And_Title_Have_Values()
        {
            // Arrange...
            var configuration = new MapperConfiguration(configure =>
            {
                configure.AddProfile<CreateUserRequestToAddUserCommand>();
            });
            var sut = configuration.CreateMapper();
            var createUserRequest = _fixture.Create<CreateUserRequest>();

            // Act...
            var addUserCommand = sut.Map<AddUserCommand>(createUserRequest);

            // Assert...
            Title? expectedTitle = DetermineTitle(createUserRequest.Title);
            addUserCommand.Title.Should().BeEquivalentTo(expectedTitle);
            addUserCommand.FirstName.Should().BeEquivalentTo(createUserRequest.FirstName);
            addUserCommand.LastName.Should().BeEquivalentTo(createUserRequest.LastName);
            Sex? expectedSex = DetermineSex(createUserRequest.Sex);
            addUserCommand.Sex.Should().BeEquivalentTo(expectedSex);
        }

        [Fact]
        public void CreateUserRequest_To_AddUserCommand_When_Sex_And_Title_Do_Not_Have_Values()
        {
            // Arrange...
            var configuration = new MapperConfiguration(configure =>
            {
                configure.AddProfile<CreateUserRequestToAddUserCommand>();
            });
            var sut = configuration.CreateMapper();
            var createUserRequest = _fixture.Create<CreateUserRequest>();
            createUserRequest.Title = null;
            createUserRequest.Sex = null;

            // Act...
            var addUserCommand = sut.Map<AddUserCommand>(createUserRequest);

            // Assert...
            addUserCommand.Title.Should().BeNull();
            addUserCommand.FirstName.Should().BeEquivalentTo(createUserRequest.FirstName);
            addUserCommand.LastName.Should().BeEquivalentTo(createUserRequest.LastName);
            addUserCommand.Sex.Should().BeNull();
        }

        #region Helper Methods

        private Sex? DetermineSex(string? value)
        {
            if (value == null) return null;
            var sex = Enum.Parse<Sex>(value);
            return sex;
        }

        private Title? DetermineTitle(string? value)
        {
            if (value == null) return null;
            var title = Enum.Parse<Title>(value);
            return title;
        }

        #endregion Helper Methods
    }
}