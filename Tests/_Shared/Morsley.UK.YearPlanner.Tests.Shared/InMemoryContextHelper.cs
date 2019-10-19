using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Morsley.UK.YearPlanner.Users.Domain.Models;
using Morsley.UK.YearPlanner.Users.Persistence.Contexts;
using System;
using System.Collections.Generic;

namespace Morsley.UK.YearPlanner.Users.Tests.Shared
{
    public static class InMemoryContextHelper
    {
        public static DataContext Create()
        {
            var builder = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging();
            var context = new DataContext(builder.Options);
            return context;
        }

        public static void AddUsersContext(DataContext inMemoryContext, IEnumerable<User> users)
        {
            foreach (var user in users)
            {
                inMemoryContext.Add(user);
            }
            inMemoryContext.SaveChanges();
        }

        public static void AddUsersContext(
            Fixture fixture,
            DataContext inMemoryContext,
            int numberOfUsers)
        {
            var users = new List<User>();
            for (int i = 0; i < numberOfUsers; i++)
            {
                var user = fixture.Create<User>();
                users.Add(user);
            }
            AddUsersContext(inMemoryContext, users);
        }
    }
}