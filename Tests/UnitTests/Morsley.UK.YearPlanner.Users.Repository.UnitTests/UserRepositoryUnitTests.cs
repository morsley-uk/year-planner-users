using AutoFixture;
using FluentAssertions;
using Morsley.UK.YearPlanner.Users.Domain.Enumerations;
using Morsley.UK.YearPlanner.Users.Persistence.Models;
using Morsley.UK.YearPlanner.Users.Persistence.Repositories;
using Morsley.UK.YearPlanner.Users.Tests.Shared;
using Morsley.UK.YearPlanner.Users.Tests.Shared.AutoFixture;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Morsley.UK.YearPlanner.Users.Repository.UnitTests
{
    public class UserRepositoryUnitTests
    {
        private readonly Fixture _fixture;

        public UserRepositoryUnitTests()
        {
            _fixture = new Fixture();
            _fixture.Customizations.Insert(0, new EnumSpecimenBuilder<Title>());
            _fixture.Customizations.Insert(1, new EnumSpecimenBuilder<Sex>());
            _fixture.Customizations.Add(new DomainUserSpecimenBuilder());
        }

        [Fact]
        public void Get_With_Null_Options_Should_Throw()
        {
            // Arrange...
            var inMemoryContext = InMemoryContextHelper.Create();
            var sut = new UserRepository(inMemoryContext);
            GetOptions getOptions = null;

            // Act...
            Func<Task> get = async () => await sut.Get(getOptions);

            // Assert...
            get.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task Get_With_Empty_Options_Should_Return_PagedList_With_No_Users_When_No_Users_Exist()
        {
            // Arrange...
            var inMemoryContext = InMemoryContextHelper.Create();
            var sut = new UserRepository(inMemoryContext);
            var getUsersOptions = new GetOptions();

            // Act...
            var pagedList = await sut.Get(getUsersOptions);

            // Assert...
            pagedList.Should().BeEmpty();
            pagedList.Count.Should().Be(0);
            pagedList.TotalCount.Should().Be(0);
            pagedList.CurrentPage.Should().Be(0);
            pagedList.TotalPages.Should().Be(0);
            pagedList.HasPrevious.Should().BeFalse();
            pagedList.HasNext.Should().BeFalse();
        }

        [Fact]
        public async Task Get_With_Empty_Options_Should_Return_PageList_With_One_User_When_Only_One_User_Exists()
        {
            // Arrange...
            var inMemoryContext = InMemoryContextHelper.Create();
            InMemoryContextHelper.AddUsersContext(_fixture, inMemoryContext, 1);
            var sut = new UserRepository(inMemoryContext);
            var getUsersOptions = new GetOptions();

            // Act...
            var pagedList = await sut.Get(getUsersOptions);

            // Assert...
            pagedList.Should().NotBeNull();
            pagedList.Count.Should().Be(1);
            pagedList.TotalCount.Should().Be(1);
            pagedList.CurrentPage.Should().Be(1);
            pagedList.TotalPages.Should().Be(1);
            pagedList.HasPrevious.Should().BeFalse();
            pagedList.HasNext.Should().BeFalse();
        }

        [Fact]
        public async Task Get_With_Pagination_Options_Should_Return_PageList_With_Page_1_When_30_Users_Exists()
        {
            // Arrange...
            var inMemoryContext = InMemoryContextHelper.Create();
            InMemoryContextHelper.AddUsersContext(_fixture, inMemoryContext, 30);
            var sut = new UserRepository(inMemoryContext);
            var getUsersOptions = new GetOptions
            {
                PageSize = 10,
                PageNumber = 1
            };

            // Act...
            var pagedList = await sut.Get(getUsersOptions);

            // Assert...
            pagedList.Should().NotBeNull();
            pagedList.Count.Should().Be(10);
            pagedList.TotalCount.Should().Be(30);
            pagedList.CurrentPage.Should().Be(1);
            pagedList.TotalPages.Should().Be(3);
            pagedList.HasPrevious.Should().BeFalse();
            pagedList.HasNext.Should().BeTrue();
        }

        [Fact]
        public async Task Get_With_Pagination_Options_Should_Return_PageList_With_Page_2_When_30_Users_Exists()
        {
            // Arrange...
            var inMemoryContext = InMemoryContextHelper.Create();
            InMemoryContextHelper.AddUsersContext(_fixture, inMemoryContext, 30);
            var sut = new UserRepository(inMemoryContext);
            var getUsersOptions = new GetOptions
            {
                PageSize = 10,
                PageNumber = 2
            };

            // Act...
            var pagedList = await sut.Get(getUsersOptions);

            // Assert...
            pagedList.Should().NotBeNull();
            pagedList.Count.Should().Be(10);
            pagedList.TotalCount.Should().Be(30);
            pagedList.CurrentPage.Should().Be(2);
            pagedList.TotalPages.Should().Be(3);
            pagedList.HasPrevious.Should().BeTrue();
            pagedList.HasNext.Should().BeTrue();
        }

        [Fact]
        public async Task Get_With_Pagination_Options_Should_Return_PageList_With_Page_3_When_30_Users_Exists()
        {
            // Arrange...
            var inMemoryContext = InMemoryContextHelper.Create();
            InMemoryContextHelper.AddUsersContext(_fixture, inMemoryContext, 30);
            var sut = new UserRepository(inMemoryContext);
            var getUsersOptions = new GetOptions
            {
                PageSize = 10,
                PageNumber = 3
            };

            // Act...
            var pagedList = await sut.Get(getUsersOptions);

            // Assert...
            pagedList.Should().NotBeNull();
            pagedList.Count.Should().Be(10);
            pagedList.TotalCount.Should().Be(30);
            pagedList.CurrentPage.Should().Be(3);
            pagedList.TotalPages.Should().Be(3);
            pagedList.HasPrevious.Should().BeTrue();
            pagedList.HasNext.Should().BeFalse();
        }

        [Fact]
        public void Find_Should_Return_User_When_User_Is_Findable()
        {
            // Arrange...
            var inMemoryContext = InMemoryContextHelper.Create();
            InMemoryContextHelper.AddUsersContext(_fixture, inMemoryContext, 1);
            var onlyUser = inMemoryContext.Users.First();
            var sut = new Persistence.Repositories.UserRepository(inMemoryContext);

            // Act...
            var foundUsers = sut.Find(u => u.FirstName == onlyUser.FirstName);

            // Assert...
            foundUsers.Should().NotBeNull();
            foundUsers.Count().Should().Be(1);
            foundUsers.First().Should().Be(onlyUser);
        }

        [Fact]
        public void Find_Should_Not_Return_User_When_User_Is_Not_Findable()
        {
            // Arrange...
            var inMemoryContext = InMemoryContextHelper.Create();
            InMemoryContextHelper.AddUsersContext(_fixture, inMemoryContext, 1);
            var sut = new Persistence.Repositories.UserRepository(inMemoryContext);
            var randomFirstName = _fixture.Create<string>();

            // Act...
            var foundUsers = sut.Find(u => u.FirstName == randomFirstName);

            // Assert...
            foundUsers.Should().NotBeNull();
            foundUsers.Count().Should().Be(0);
        }

        [Fact]
        public async Task Filter_Should_Include_Users_That_Match_Filter()
        {
            // Arrange...
            var inMemoryContext = InMemoryContextHelper.Create();
            InMemoryContextHelper.AddUsersContext(_fixture, inMemoryContext, 1);
            var onlyUser = inMemoryContext.Users.First();
            var sut = new Persistence.Repositories.UserRepository(inMemoryContext);
            var getOptions = new GetOptions();
            var filter = new Filter("Title", onlyUser.Title.ToString());
            getOptions.AddFilter(filter);

            // Act...
            var pageOfUsers = await sut.Get(getOptions);

            // Assert...
            pageOfUsers.Should().NotBeNull();
            pageOfUsers.Count.Should().Be(1);
        }

        [Fact]
        public async Task Filter_Should_Exclude_Users_That_Do_Not_Match_Filter()
        {
            // Arrange...
            var inMemoryContext = InMemoryContextHelper.Create();
            InMemoryContextHelper.AddUsersContext(_fixture, inMemoryContext, 1);
            var onlyUser = inMemoryContext.Users.First();
            var sut = new Persistence.Repositories.UserRepository(inMemoryContext);
            var getOptions = new GetOptions();
            var filter = new Filter("Title", GetDifferent(onlyUser.Title).ToString());
            getOptions.AddFilter(filter);

            // Act...
            var pageOfUsers = await sut.Get(getOptions);

            // Assert...
            pageOfUsers.Should().NotBeNull();
            pageOfUsers.Count.Should().Be(0);

        }

        [Fact]
        public async Task Search_Should_Find_Users_That_Match_Search()
        {
            // Arrange...
            var inMemoryContext = InMemoryContextHelper.Create();
            InMemoryContextHelper.AddUsersContext(_fixture, inMemoryContext, 2);
            var onlyUser = inMemoryContext.Users.First();
            var sut = new Persistence.Repositories.UserRepository(inMemoryContext);
            var getOptions = new GetOptions();
            getOptions.SearchQuery = onlyUser.FirstName;

            // Act...
            var pageOfUsers = await sut.Get(getOptions);

            // Assert...
            pageOfUsers.Should().NotBeNull();
            pageOfUsers.Count.Should().Be(1);
            pageOfUsers.TotalCount.Should().Be(1);
        }

        [Fact]
        public async Task Search_Should_Not_Find_Users_That_Do_Not_Match_Search()
        {
            // Arrange...
            var inMemoryContext = InMemoryContextHelper.Create();
            InMemoryContextHelper.AddUsersContext(_fixture, inMemoryContext, 2);
            var onlyUser = inMemoryContext.Users.First();
            var sut = new Persistence.Repositories.UserRepository(inMemoryContext);
            var getOptions = new GetOptions();
            getOptions.SearchQuery = _fixture.Create<string>();

            // Act...
            var pageOfUsers = await sut.Get(getOptions);

            // Assert...
            pageOfUsers.Should().NotBeNull();
            pageOfUsers.Count.Should().Be(0);
            pageOfUsers.TotalCount.Should().Be(0);

        }

        [Fact]
        public async Task Sort_Should_Sort_Users_Into_Correct_Order_When_Descending()
        {
            // Arrange...
            var inMemoryContext = InMemoryContextHelper.Create();
            InMemoryContextHelper.AddUsersContext(_fixture, inMemoryContext, 3);
            var sut = new UserRepository(inMemoryContext);
            var getOptions = new GetOptions();
            var ordering = new Ordering("FirstName", SortOrder.Descending);
            getOptions.AddOrdering(ordering);

            // Act...
            var pageOfUsers = await sut.Get(getOptions);

            // Assert...
            pageOfUsers.Should().NotBeNull();
            pageOfUsers.Count.Should().Be(3);
            pageOfUsers.TotalCount.Should().Be(3);
            var orderedInMemoryUsers = inMemoryContext.Users.OrderByDescending(x => x.FirstName);
            var firstUser = orderedInMemoryUsers.Skip(0).Take(1);
            var secondUser = orderedInMemoryUsers.Skip(1).Take(1);
            var thirdUser = orderedInMemoryUsers.Skip(2).Take(1);
            pageOfUsers.Skip(0).Take(1).Should().BeEquivalentTo(firstUser);
            pageOfUsers.Skip(1).Take(1).Should().BeEquivalentTo(secondUser);
            pageOfUsers.Skip(2).Take(1).Should().BeEquivalentTo(thirdUser);
        }

        [Fact]
        public async Task Sort_Should_Sort_Users_Into_Correct_Order_When_Ascending()
        {
            // Arrange...
            var inMemoryContext = InMemoryContextHelper.Create();
            InMemoryContextHelper.AddUsersContext(_fixture, inMemoryContext, 3);
            var sut = new UserRepository(inMemoryContext);
            var getOptions = new GetOptions();
            var ordering = new Ordering("FirstName", SortOrder.Ascending);
            getOptions.AddOrdering(ordering);

            // Act...
            var pageOfUsers = await sut.Get(getOptions);

            // Assert...
            pageOfUsers.Should().NotBeNull();
            pageOfUsers.Count.Should().Be(3);
            pageOfUsers.TotalCount.Should().Be(3);
            var orderedInMemoryUsers = inMemoryContext.Users.OrderBy(x => x.FirstName);
            var firstUser = orderedInMemoryUsers.Skip(0).Take(1);
            var secondUser = orderedInMemoryUsers.Skip(1).Take(1);
            var thirdUser = orderedInMemoryUsers.Skip(2).Take(1);
            pageOfUsers.Skip(0).Take(1).Should().BeEquivalentTo(firstUser);
            pageOfUsers.Skip(1).Take(1).Should().BeEquivalentTo(secondUser);
            pageOfUsers.Skip(2).Take(1).Should().BeEquivalentTo(thirdUser);
        }

        #region Helper Methods

        private Title GetDifferent(Title original)
        {
            do
            {
                var different = _fixture.Create<Title>();
                if (different != original) return different;
            } while (true);
        }

        #endregion Helper Methods
    }
}