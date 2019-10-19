using AutoFixture;
using FluentAssertions;
using Morsley.UK.YearPlanner.Users.Domain.Interfaces;
using Morsley.UK.YearPlanner.Users.Domain.Models;
using Morsley.UK.YearPlanner.Users.Persistence.Repositories;
using Morsley.UK.YearPlanner.Users.Tests.Shared;
using Morsley.UK.YearPlanner.Users.Tests.Shared.AutoFixture;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Morsley.UK.YearPlanner.Users.UnitOfWork.IntegrationTests
{
    public class UnitOfWorkIntegrationTests
    {
        private readonly Fixture _fixture;

        public UnitOfWorkIntegrationTests()
        {
            _fixture = new Fixture();
            _fixture.Customizations.Add(new DomainUserSpecimenBuilder());
            _fixture.Customizations.Add(new TitleSpecimenBuilder());
        }

        [Fact]
        public async Task Create_Should_Be_Able_To_Add_User()
        {
            // Arrange...
            var user = _fixture.Create<User>();
            var inMemoryContext = InMemoryContextHelper.Create();
            var userRepository = new UserRepository(inMemoryContext);
            var dt = _fixture.Create<DateTime>();
            var mockDateTimeService = MockDateTimeService(dt);
            var sut = new Persistence.UnitOfWork(inMemoryContext, mockDateTimeService, userRepository);

            // Act...
            sut.UserRepository.Create(user);
            await sut.CompleteAsync();

            // Assert...
            inMemoryContext.Users.Count().Should().Be(1);
            inMemoryContext.Users.First().Should().Be(user);
            user.Id.Should().NotBe(default);
            user.Created.Should().NotBe(default);
            user.Updated.Should().BeNull();
            sut.Dispose();
        }

        [Fact]
        public async Task Should_Be_Able_To_Update_A_User()
        {
            // Arrange...
            var inMemoryContext = InMemoryContextHelper.Create();
            var user = _fixture.Create<User>();
            inMemoryContext.Users.Add(user);
            inMemoryContext.SaveChanges();
            var userId = user.Id;
            var userRepository = new UserRepository(inMemoryContext);
            var dt = _fixture.Create<DateTime>();
            var mockDateTimeService = MockDateTimeService(dt);
            var sut = new Persistence.UnitOfWork(inMemoryContext, mockDateTimeService, userRepository);

            // Act...
            var update = await sut.UserRepository.Get(userId);
            var newFirstName = _fixture.Create<string>();
            update.FirstName = newFirstName;
            await sut.CompleteAsync();

            // Assert...
            update.Updated.Should().NotBeNull();
            update.Updated.Should().Be(dt);
            update.FirstName.Should().Be(newFirstName);
            sut.Dispose();
        }

        [Fact]
        public async Task Delete_Should_Delete_User_With_Valid_ID()
        {
            // Arrange...
            var inMemoryContext = InMemoryContextHelper.Create();
            var userRepository = new UserRepository(inMemoryContext);
            var dt = _fixture.Create<DateTime>();
            var mockDateTimeService = MockDateTimeService(dt);
            var sut = new Persistence.UnitOfWork(inMemoryContext, mockDateTimeService, userRepository);
            var user = _fixture.Create<User>();
            inMemoryContext.Users.Add(user);
            inMemoryContext.SaveChanges();

            // Act...
            sut.UserRepository.Delete(user.Id);
            var numberOfRowsAffected = await sut.CompleteAsync();

            // Assert...
            numberOfRowsAffected.Should().Be(1);
        }

        [Fact]
        public async Task Delete_Should_Delete_User_With_Valid_Entity()
        {
            // Arrange...
            var inMemoryContext = InMemoryContextHelper.Create();
            var userRepository = new UserRepository(inMemoryContext);
            var dt = _fixture.Create<DateTime>();
            var mockDateTimeService = MockDateTimeService(dt);
            var sut = new Persistence.UnitOfWork(inMemoryContext, mockDateTimeService, userRepository);
            var user = _fixture.Create<User>();
            inMemoryContext.Users.Add(user);
            inMemoryContext.SaveChanges();

            // Act...
            sut.UserRepository.Delete(user);
            var numberOfRowsAffected = await sut.CompleteAsync();

            // Assert...
            numberOfRowsAffected.Should().Be(1);
        }

        #region Helper Methods

        //private void AddUsersContext(DataContext inMemoryContext, int numberOfUsers)
        //{
        //    for (int i = 0; i < numberOfUsers; i++)
        //    {
        //        var user = _fixture.Create<User>();
        //        inMemoryContext.Users.Add(user);
        //    }
        //    inMemoryContext.SaveChanges();
        //}

        //private DataContext Create()
        //{
        //    var builder = new DbContextOptionsBuilder<DataContext>()
        //        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        //        .EnableSensitiveDataLogging();
        //    var context = new DataContext(builder.Options);
        //    return context;
        //}

        private static IDateTimeService MockDateTimeService()
        {
            var service = new MockDateTimeService();
            return service;
        }

        private static IDateTimeService MockDateTimeService(DateTime now)
        {
            var service = new MockDateTimeService();
            service.SetDateTimeUtcNow(now);
            return service;
        }

        private static void UpdateMockDateTimeService(IDateTimeService service, DateTime update)
        {
            ((MockDateTimeService)service).SetDateTimeUtcNow(update);
        }

        #endregion Helper Methods
    }
}