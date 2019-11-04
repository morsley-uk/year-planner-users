using AutoFixture;
using FluentAssertions;
using Morsley.UK.YearPlanner.Users.Domain.Models;
using Morsley.UK.YearPlanner.Users.Persistence.Repositories;
using Morsley.UK.YearPlanner.Users.Tests.Shared;
using Morsley.UK.YearPlanner.Users.Tests.Shared.AutoFixture;
using System;
using System.Linq;
using System.Threading.Tasks;
using Morsley.UK.YearPlanner.Users.Domain.Enumerations;
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
            var mockDateTimeService = MockDateTimeServiceHelper.Create(dt);
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
            var mockDateTimeService = MockDateTimeServiceHelper.Create(dt);
            var sut = new Persistence.UnitOfWork(inMemoryContext, mockDateTimeService, userRepository);

            // Act...
            var update = await sut.UserRepository.Get(userId);
            var newTitle = GetDifferentValue(update.Title);
            update.Title = newTitle;
            var newFirstName = _fixture.Create<string>();
            update.FirstName = newFirstName;
            var newLastName = _fixture.Create<string>();
            update.LastName = newLastName;
            var newSex = GetDifferentValue(update.Sex);
            update.Sex = newSex;
            await sut.CompleteAsync();

            // Assert...
            update.Updated.Should().NotBeNull();
            update.Updated.Should().Be(dt);
            update.Title.Should().Be(newTitle);
            update.FirstName.Should().Be(newFirstName);
            update.LastName.Should().Be(newLastName);
            update.Sex.Should().Be(newSex);

            sut.Dispose();
        }

        private T GetDifferentValue<T>(T oldValue)
        {
            T newValue;

            do
            {
                newValue = _fixture.Create<T>();
            } while (newValue.Equals(oldValue));

            return newValue;
        }

        //private Sex? GetNewSex(Sex? oldSex)
        //{
        //    Sex? newSex;

        //    do
        //    {
        //        newSex = _fixture.Create<Sex?>();
        //    } while (newSex == oldSex);

        //    return newSex;
        //}

        //private Title? GetNewTitle(Title? oldTitle)
        //{
        //    Title? newTitle;

        //    do
        //    {
        //        newTitle = _fixture.Create<Title?>();
        //    } while (newTitle == oldTitle);

        //    return newTitle;

        //}

        [Fact]
        public async Task Delete_Should_Delete_User_With_Valid_ID()
        {
            // Arrange...
            var inMemoryContext = InMemoryContextHelper.Create();
            var userRepository = new UserRepository(inMemoryContext);
            var dt = _fixture.Create<DateTime>();
            var mockDateTimeService = MockDateTimeServiceHelper.Create(dt);
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
            var mockDateTimeService = MockDateTimeServiceHelper.Create(dt);
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

        #endregion Helper Methods
    }
}