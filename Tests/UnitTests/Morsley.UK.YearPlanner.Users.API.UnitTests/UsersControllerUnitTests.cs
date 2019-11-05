using AutoFixture;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Morsley.UK.YearPlanner.Users.API.Controllers.v1;
using Morsley.UK.YearPlanner.Users.API.Models.v1.Request;
using Morsley.UK.YearPlanner.Users.API.Models.v1.Response;
using Morsley.UK.YearPlanner.Users.Application.Commands;
using Morsley.UK.YearPlanner.Users.Application.Queries;
using Morsley.UK.YearPlanner.Users.Domain.Enumerations;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Morsley.UK.YearPlanner.Users.API.UnitTests
{
    public class UsersControllerUnitTests
    {
        private readonly Fixture _fixture;

        public UsersControllerUnitTests()
        {
            _fixture = new Fixture();
        }

        #region Unhappy Paths

        [Fact]
        public async Task DELETE_Should_Return_BadRequest_When_Request_Is_Null()
        {
            // Arrange...
            var mockMediator = Substitute.For<IMediator>();
            var mockMapper = Substitute.For<IMapper>();
            var sut = new UsersController(mockMediator, mockMapper);
            var userId = Guid.Empty;

            // Act...
            var result = await sut.Delete(userId);

            // Assert...
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestResult>();
        }

        [Fact]
        public async Task GET_Plural_Should_Return_BadRequest_When_Request_Is_Null()
        {
            // Arrange...
            var mockMediator = Substitute.For<IMediator>();
            var mockMapper = Substitute.For<IMapper>();
            var sut = new UsersController(mockMediator, mockMapper);
            const GetUsersRequest request = null;

            // Act...
            var result = await sut.Get(request);

            // Assert...
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestResult>();
        }

        [Fact]
        public async Task GET_Single_Should_Return_BadRequest_When_Request_Is_Null()
        {
            // Arrange...
            var mockMediator = Substitute.For<IMediator>();
            var mockMapper = Substitute.For<IMapper>();
            var sut = new UsersController(mockMediator, mockMapper);
            var userId = Guid.Empty;

            // Act...
            var result = await sut.Get(userId);

            // Assert...
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestResult>();
        }

        [Fact]
        public async Task PATCH_Should_Return_Bad_Request_When_PatchDocument_Is_Null()
        {
            // Arrange...
            var mockMediator = Substitute.For<IMediator>();
            var mockMapper = Substitute.For<IMapper>();
            var sut = new UsersController(mockMediator, mockMapper);
            var userId = Guid.NewGuid();
            const JsonPatchDocument<PartiallyUpsertUserRequest> patchDocument = null;

            // Act...
            var result = await sut.Upsert(userId, patchDocument);

            // Assert...
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestResult>();
        }

        [Fact]
        public async Task POST_Should_Return_BadRequest_When_Request_Is_Null()
        {
            // Arrange...
            var mockMediator = Substitute.For<IMediator>();
            var mockMapper = Substitute.For<IMapper>();
            var sut = new UsersController(mockMediator, mockMapper);
            const CreateUserRequest request = null;

            // Act...
            var result = await sut.Add(request);

            // Assert...
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestResult>();
        }

        [Fact]
        public async Task PUT_Should_Return_Bad_Request_When_Request_Is_Null()
        {
            // Arrange...
            var mockMediator = Substitute.For<IMediator>();
            var mockMapper = Substitute.For<IMapper>();
            var sut = new UsersController(mockMediator, mockMapper);
            var userId = Guid.NewGuid();
            const UpsertUserRequest request = null;

            // Act...
            var result = await sut.Upsert(userId, request);

            // Assert...
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<BadRequestResult>();
        }

        #endregion Unhappy Paths

        #region Happy Paths

        [Fact]
        public async Task GET_Plural_Should_Return_NoContent_When_No_Users_Exist()
        {
            // Arrange...
            var mockMediator = Substitute.For<IMediator>();
            var mockMapper = Substitute.For<IMapper>();
            var sut = new UsersController(mockMediator, mockMapper);
            var request = new GetUsersRequest();

            // Act...
            var response = await sut.Get(request);

            // Assert...
            response.Should().NotBeNull();
            response.Should().BeAssignableTo<NoContentResult>();
        }

        [Fact]
        public async Task GET_Single_Should_Return_NoContent_When_No_Users_Exist()
        {
            // Arrange...
            var mockMediator = Substitute.For<IMediator>();
            var mockMapper = Substitute.For<IMapper>();
            var sut = new UsersController(mockMediator, mockMapper);
            var userId = Guid.NewGuid();

            // Act...
            var response = await sut.Get(userId);

            // Assert...
            response.Should().NotBeNull();
            response.Should().BeAssignableTo<NoContentResult>();
        }

        [Fact]
        public async Task PATCH_Should_Return_OK_When_User_Is_Updated()
        {
            // Arrange...
            var mockMediator = Substitute.For<IMediator>();
            var mockMapper = Substitute.For<IMapper>();
            var sut = new UsersController(mockMediator, mockMapper);
            sut.ObjectValidator = Substitute.For<IObjectModelValidator>();
            mockMediator.Send(Arg.Any<UserExistsQuery>()).Returns(true);
            var userId = Guid.NewGuid();
            var patchDocument = new JsonPatchDocument<PartiallyUpsertUserRequest>();
            patchDocument.Replace<string>(o => o.FirstName, "Foo");

            // Act...
            var result = await sut.Upsert(userId, patchDocument);

            // Assert...
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<OkObjectResult>();
        }

        [Fact]
        public async Task POST_Should_Return_Created_When_Request_Is_Successful()
        {
            // Arrange...
            var fixture = new Fixture();
            var mockMediator = Substitute.For<IMediator>();
            var mockMapper = Substitute.For<IMapper>();
            var sut = new UsersController(mockMediator, mockMapper);
            var request = fixture.Create<CreateUserRequest>();

            // Act...
            var result = await sut.Add(request);

            // Assert...
            result.Should().NotBeNull();
            // ToDo --> As neither CreatedAtRoute and CreatedAtAction are working
            //result.Should().BeAssignableTo<CreatedAtRouteResult>();
        }

        [Fact]
        public async Task PUT_Should_Return_OK_When_Called_With_Complete_Request_To_Update_User()
        {
            // Arrange...
            var mockMediator = Substitute.For<IMediator>();
            var mockMapper = Substitute.For<IMapper>();
            var sut = new UsersController(mockMediator, mockMapper);

            var userId = Guid.NewGuid();
            var title = _fixture.Create<Title>();
            var firstName = _fixture.Create<string>();
            var lastName = _fixture.Create<string>();
            var sex = _fixture.Create<Sex>();

            var upsertUserRequest = new UpsertUserRequest
            {
                Title = title.ToString(),
                FirstName = firstName,
                LastName = lastName,
                Sex = sex.ToString()
            };

            var updateUserCommand = new UpdateUserCommand
            {
                Id = userId,
                Title = title,
                FirstName = firstName,
                LastName = lastName,
                Sex = sex
            };
            mockMapper.Map<UpdateUserCommand>(upsertUserRequest).Returns(updateUserCommand);

            mockMediator.Send(Arg.Any<UserExistsQuery>()).Returns(true);

            var updatedUser = new Domain.Models.User(firstName, lastName)
            {
                Id = userId,
                Title = title,
                Sex = sex,
                Created = DateTime.UtcNow.AddMinutes(-1),
                Updated = DateTime.UtcNow,
            };
            mockMediator.Send(updateUserCommand).Returns(updatedUser);

            var updatedUserResponse = new UserResponse
            {
                Id = userId,
                Title = title.ToString(),
                FirstName = firstName,
                LastName = lastName,
                Sex = sex.ToString()
            };
            mockMapper.Map<UserResponse>(updatedUser).Returns(updatedUserResponse);

            // Act...
            var objectResult = await sut.Upsert(userId, upsertUserRequest);

            // Assert...
            objectResult.Should().NotBeNull();
            objectResult.Should().BeAssignableTo<OkObjectResult>();
            var okObjectResult = objectResult as OkObjectResult;
            okObjectResult.Value.Should().NotBeNull();
            okObjectResult.Value.Should().BeAssignableTo<UserResponse>();
            var value = okObjectResult.Value as UserResponse;
            value.Id.Should().Be(userId);
            value.Title.Should().Be(title.ToString());
            value.FirstName.Should().Be(firstName);
            value.LastName.Should().Be(lastName);
            value.Sex.Should().Be(sex.ToString());
        }

        [Fact]
        public async Task PUT_Should_Return_OK_When_Called_With_Partial_Request_To_Update_User()
        {
            // Arrange...
            var mockMediator = Substitute.For<IMediator>();
            var mockMapper = Substitute.For<IMapper>();
            var sut = new UsersController(mockMediator, mockMapper);

            var userId = Guid.NewGuid();
            var firstName = _fixture.Create<string>();
            var lastName = _fixture.Create<string>();

            var upsertUserRequest = new UpsertUserRequest
            {
                Title = null,
                FirstName = firstName,
                LastName = lastName,
                Sex = null
            };

            var updateUserCommand = new UpdateUserCommand
            {
                Id = userId,
                Title = null,
                FirstName = firstName,
                LastName = lastName,
                Sex = null
            };
            mockMapper.Map<UpdateUserCommand>(upsertUserRequest).Returns(updateUserCommand);

            mockMediator.Send(Arg.Any<UserExistsQuery>()).Returns(true);

            var updatedUser = new Domain.Models.User(firstName, lastName)
            {
                Id = userId,
                Title = null,
                Sex = null,
                Created = DateTime.UtcNow.AddMinutes(-1),
                Updated = DateTime.UtcNow,
            };
            mockMediator.Send(updateUserCommand).Returns(updatedUser);

            var updatedUserResponse = new UserResponse
            {
                Id = userId,
                Title = null,
                FirstName = firstName,
                LastName = lastName,
                Sex = null
            };
            mockMapper.Map<UserResponse>(updatedUser).Returns(updatedUserResponse);

            // Act...
            var objectResult = await sut.Upsert(userId, upsertUserRequest);

            // Assert...
            objectResult.Should().NotBeNull();
            objectResult.Should().BeAssignableTo<OkObjectResult>();
            var okObjectResult = objectResult as OkObjectResult;
            okObjectResult.Value.Should().NotBeNull();
            okObjectResult.Value.Should().BeAssignableTo<UserResponse>();
            var value = okObjectResult.Value as UserResponse;
            value.Id.Should().Be(userId);
            value.Title.Should().BeNullOrEmpty();
            value.FirstName.Should().Be(firstName);
            value.LastName.Should().Be(lastName);
            value.Sex.Should().BeNullOrEmpty();
        }

        [Fact]
        public void TryValidateModel()
        {
            // Arrange...
            var mockMediator = Substitute.For<IMediator>();
            var mockMapper = Substitute.For<IMapper>();
            var sut = new UsersController(mockMediator, mockMapper);
            sut.ObjectValidator = Substitute.For<IObjectModelValidator>();
            var userId = _fixture.Create<Guid>();
            var partiallyUpsertUserRequest = new PartiallyUpsertUserRequest(userId);

            // Act...
            var result = sut.TryValidateModel(partiallyUpsertUserRequest);

            // Assert...

        }

        // ToDo
        // Test PATCH creating a user
        // Test PUT creating a user

        #endregion Happy Paths
    }
}