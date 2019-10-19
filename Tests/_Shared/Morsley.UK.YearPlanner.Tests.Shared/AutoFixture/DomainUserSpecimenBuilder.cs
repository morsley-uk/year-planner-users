using AutoFixture;
using AutoFixture.Kernel;
using Morsley.UK.YearPlanner.Users.Domain.Enumerations;
using System;

namespace Morsley.UK.YearPlanner.Users.Tests.Shared.AutoFixture
{
    public class DomainUserSpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            var seededRequest = request as SeededRequest;

            if (seededRequest == null) return new NoSpecimen();

            var type = seededRequest.Request as Type;

            if (type == null) return new NoSpecimen();

            if (type != typeof(Domain.Models.User)) return new NoSpecimen();

            var user = new Domain.Models.User();

            user.Title = context.Create<Title>();
            user.FirstName = nameof(user.FirstName) + "___" + context.Create<string>();
            user.LastName = nameof(user.LastName) + "___" + context.Create<string>();
            user.Sex = context.Create<Sex>();

            user.Created = DateTime.MinValue;
            user.Updated = null;
            user.Deleted = null;

            return user;
        }

    }
}