using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Morsley.UK.YearPlanner.Users.API.Models.v1.Request;
using Morsley.UK.YearPlanner.Users.API.Models.v1.Response;
using Morsley.UK.YearPlanner.Users.API.Profiles;
using Morsley.UK.YearPlanner.Users.API.UnitTests.AutoFixture;
using Morsley.UK.YearPlanner.Users.Application.Commands;
using Morsley.UK.YearPlanner.Users.Application.Queries;
using Morsley.UK.YearPlanner.Users.Domain.Enumerations;
using Morsley.UK.YearPlanner.Users.Domain.Models;
using Morsley.UK.YearPlanner.Users.Tests.Shared.AutoFixture;
using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;

namespace Morsley.UK.YearPlanner.Users.API.UnitTests
{
    public class AutoMapperUnitTests
    {
        private readonly Fixture _fixture;

        public AutoMapperUnitTests()
        {
            _fixture = new Fixture();
            _fixture.Customizations.Insert(0, new EnumSpecimenBuilder<Title>());
            _fixture.Customizations.Insert(1, new EnumSpecimenBuilder<Sex>());
            _fixture.Customizations.Add(new CreateUserRequestSpecimenBuilder());
            _fixture.Customizations.Add(new DomainUserSpecimenBuilder());
        }

        [Fact]
        public void CreateUserRequest_To_AddUserCommand_With_Sex_And_Title()
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
        public void CreateUserRequest_To_AddUserCommand_Without_Sex_And_Title()
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

        [Fact]
        public void DeleteUserRequest_To_DeleteUserCommand()
        {
            // Arrange...
            var configuration = new MapperConfiguration(configure =>
            {
                configure.AddProfile<DeleteUserRequestToDeleteUserCommand>();
            });
            var sut = configuration.CreateMapper();
            var deleteUserRequest = _fixture.Create<GetUserRequest>();

            // Act...
            var deleteUserCommand = sut.Map<DeleteUserCommand>(deleteUserRequest);

            // Assert...
            deleteUserCommand.Should().BeEquivalentTo(deleteUserRequest);
        }

        [Fact]
        public void GetUserRequest_To_GetUserQuery()
        {
            // Arrange...
            var configuration = new MapperConfiguration(configure =>
            {
                configure.AddProfile<GetUserRequestToGetUserQuery>();
            });
            var sut = configuration.CreateMapper();
            var getUserRequest = _fixture.Create<GetUserRequest>();

            // Act...
            var getUserQuery = sut.Map<GetUserQuery>(getUserRequest);

            // Assert...
            getUserQuery.Should().BeEquivalentTo(getUserRequest);
        }

        [Fact]
        public void GetUsersRequest_To_GetUsersQuery()
        {
            // Arrange...
            var configuration = new MapperConfiguration(configure =>
            {
                configure.AddProfile<GetUsersRequestToGetUsersQuery>();
            });
            var sut = configuration.CreateMapper();
            var getUsersRequest = _fixture.Create<GetUsersRequest>();

            // Act...
            var getUsersQuery = sut.Map<GetUsersQuery>(getUsersRequest);

            // Assert...
            getUsersQuery.Should().BeEquivalentTo(getUsersRequest);
        }

        [Fact]
        public void PartiallyUpdateUserRequest_To_PartiallyUpdateUserCommand()
        {
            // Arrange...
            var configuration = new MapperConfiguration(configure =>
            {
                configure.AddProfile<PartiallyUpdateUserRequestToPartiallyUpdateUserCommand>();
            });
            var sut = configuration.CreateMapper();
            var userId = _fixture.Create<Guid>();
            var firstName = _fixture.Create<string>();
            var partiallyUpdateUserRequest = new PartiallyUpdateUserRequest(userId)
            {
                FirstName = firstName
            };

            // Act...
            var partiallyUpdateUserCommand = sut.Map<PartiallyUpdateUserCommand>(partiallyUpdateUserRequest);

            // Assert...
            partiallyUpdateUserCommand.Id.Should().Be(userId);
            partiallyUpdateUserCommand.Title.Should().BeNull();
            partiallyUpdateUserCommand.FirstName.Should().Be(firstName);
            partiallyUpdateUserCommand.LastName.Should().BeNull();
            partiallyUpdateUserCommand.Sex.Should().BeNull();
        }

        [Fact]
        public void Persistence_PagedList_To_API_PagedList()
        {
            // Arrange...
            _fixture.RepeatCount = 10;
            var users = _fixture.Create<List<User>>();
            var pagedListUsers = Substitute.For<Domain.Interfaces.IPagedList<User>>();
            pagedListUsers.CurrentPage.Returns(1);
            pagedListUsers.PageSize.Returns(10);
            pagedListUsers.HasPrevious.Returns(false);
            pagedListUsers.HasNext.Returns(true);
            pagedListUsers.TotalCount.Returns(200);
            pagedListUsers.TotalPages.Returns(20);
            pagedListUsers.GetEnumerator().Returns(users.GetEnumerator());

            var configuration = new MapperConfiguration(configure =>
            {
                configure.AddProfile<PersistencePagedListToApiPagedList>();
            });
            var sut = configuration.CreateMapper();

            // Act...
            var pagedListUserResponses = sut.Map<API.Models.v1.PagedList<UserResponse>>(pagedListUsers);

            // Assert...
            pagedListUserResponses.Count.Should().Be(pagedListUsers.PageSize);
            pagedListUserResponses.CurrentPage.Should().Be(pagedListUsers.CurrentPage);
            pagedListUserResponses.PageSize.Should().Be(pagedListUsers.PageSize);
            pagedListUserResponses.HasPrevious.Should().Be(pagedListUsers.HasPrevious);
            pagedListUserResponses.HasNext.Should().Be(pagedListUsers.HasNext);
            pagedListUserResponses.TotalCount.Should().Be(pagedListUsers.TotalCount);
            pagedListUserResponses.TotalPages.Should().Be(pagedListUsers.TotalPages);
        }

        [Fact]
        public void UpdateUserRequest_To_UpdateUserCommand()
        {
            // Arrange...
            var configuration = new MapperConfiguration(configure =>
            {
                configure.AddProfile<UpdateUserRequestToUpdateUserCommand>();
            });
            var sut = configuration.CreateMapper();
            var updateUserRequest = _fixture.Create<UpdateUserCommand>();

            // Act...
            var updateUserCommand = sut.Map<UpdateUserCommand>(updateUserRequest);

            // Assert...
            updateUserCommand.Should().BeEquivalentTo(updateUserRequest);
        }

        [Fact]
        public void UpdateUserRequest_To_UpdateUserCommand_With_Nulls()
        {
            // Arrange...
            var configuration = new MapperConfiguration(configure =>
            {
                configure.AddProfile<UpdateUserRequestToUpdateUserCommand>();
            });
            var sut = configuration.CreateMapper();
            var updateUserRequest = _fixture.Create<UpdateUserCommand>();
            updateUserRequest.Title = null;
            updateUserRequest.Sex = null;

            // Act...
            var updateUserCommand = sut.Map<UpdateUserCommand>(updateUserRequest);

            // Assert...
            updateUserCommand.Should().BeEquivalentTo(updateUserRequest);
        }

        [Fact]
        public void User_To_UserResponse_With_Sex_And_Title()
        {
            // Arrange...
            var configuration = new MapperConfiguration(configure =>
            {
                configure.AddProfile<UserToUserResponse>();
            });
            var sut = configuration.CreateMapper();
            var user = _fixture.Create<User>();

            // Act...
            var userResponse = sut.Map<UserResponse>(user);

            // Assert...
            var expectedTitle = DetermineTitle(user.Title);
            userResponse.Title.Should().Be(expectedTitle);
            userResponse.FirstName.Should().BeEquivalentTo(user.FirstName);
            userResponse.LastName.Should().BeEquivalentTo(user.LastName);
            var expectedSex = DetermineSex(user.Sex);
            userResponse.Sex.Should().Be(expectedSex);
        }

        [Fact]
        public void User_To_UserResponse_Without_Sex_And_Title()
        {
            // Arrange...
            var configuration = new MapperConfiguration(configure =>
            {
                configure.AddProfile<UserToUserResponse>();
            });
            var sut = configuration.CreateMapper();
            var user = _fixture.Create<User>();
            user.Title = null;
            user.Sex = null;

            // Act...
            var userResponse = sut.Map<UserResponse>(user);

            // Assert...
            userResponse.FirstName.Should().BeEquivalentTo(user.FirstName);
            userResponse.LastName.Should().BeEquivalentTo(user.LastName);
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

        private string? DetermineSex(Sex? value)
        {
            if (value == null) return null;
            var sex = value.ToString();
            return sex;
        }

        private string? DetermineTitle(Title? value)
        {
            if (value == null) return null;
            var title = value.ToString();
            return title;
        }

        #endregion Helper Methods
    }
}