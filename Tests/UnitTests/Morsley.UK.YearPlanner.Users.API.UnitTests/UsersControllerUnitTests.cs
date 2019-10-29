using AutoFixture;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Morsley.UK.YearPlanner.Users.API.Controllers.v1;
using Morsley.UK.YearPlanner.Users.API.Models.v1.Request;
using NSubstitute;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xunit;

namespace Morsley.UK.YearPlanner.Users.API.UnitTests
{
    public class UsersControllerUnitTests
    {
        //private readonly Fixture _fixture;

        //public UsersControllerUnitTests()
        //{
        //    _fixture = new Fixture();
        //}

        public class UnhappyPath
        {
            [Fact]
            [Category("Unhappy Path")]
            //[Trait("Category", "Unhappy Path")]
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
                const GetUserRequest request = null;

                // Act...
                var result = await sut.Get(request);

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
            public async Task DELETE_Should_Return_BadRequest_When_Request_Is_Null()
            {
                // Arrange...
                var mockMediator = Substitute.For<IMediator>();
                var mockMapper = Substitute.For<IMapper>();
                var sut = new UsersController(mockMediator, mockMapper);
                const DeleteUserRequest request = null;

                // Act...
                var result = await sut.Delete(request);

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
                const UpdateUserRequest request = null;

                // Act...
                var result = await sut.Update(userId, request);

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
                //const JsonPatchDocument<UpdateUserRequest> patchDocument = null;
                const PartiallyUpdateUserRequest request = null;

                // Act...
                //var result = sut.Update(userId, patchDocument);
                var result = await sut.Update(userId, request);

                // Assert...
                result.Should().NotBeNull();
                result.Should().BeAssignableTo<BadRequestResult>();
            }
        }

        public class HappyPath
        {
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
                var request = new GetUserRequest();

                // Act...
                var response = await sut.Get(request);

                // Assert...
                response.Should().NotBeNull();
                response.Should().BeAssignableTo<NoContentResult>();
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
                result.Should().BeAssignableTo<CreatedAtRouteResult>();
            }
        }
    }
}