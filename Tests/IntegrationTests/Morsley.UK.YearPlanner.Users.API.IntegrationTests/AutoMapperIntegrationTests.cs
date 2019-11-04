using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Morsley.UK.YearPlanner.Users.API.Models.v1.Response;
using Morsley.UK.YearPlanner.Users.API.Profiles;
using Morsley.UK.YearPlanner.Users.Domain.Enumerations;
using Morsley.UK.YearPlanner.Users.Domain.Models;
using Morsley.UK.YearPlanner.Users.Tests.Shared;
using Morsley.UK.YearPlanner.Users.Tests.Shared.AutoFixture;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Morsley.UK.YearPlanner.Users.API.IntegrationTests
{
    public class AutoMapperIntegrationTests
    {
        private readonly Fixture _fixture;

        public AutoMapperIntegrationTests()
        {
            _fixture = new Fixture();
            _fixture.Customizations.Insert(0, new EnumSpecimenBuilder<Title>());
            _fixture.Customizations.Insert(1, new EnumSpecimenBuilder<Sex>());
            _fixture.Customizations.Add(new DomainUserSpecimenBuilder());
        }

        [Fact]
        public async Task Persistence_PagedList_To_API_PagedList()
        {
            // Arrange...
            _fixture.RepeatCount = 10;
            var users = _fixture.Create<List<User>>();
            var inMemoryContext = InMemoryContextHelper.Create();
            InMemoryContextHelper.AddUsersToContext(inMemoryContext, users);
            var pagedListUsers = await Persistence.Models.PagedList<User>.CreateAsync(inMemoryContext.Users, 1, 5);

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
    }
}
