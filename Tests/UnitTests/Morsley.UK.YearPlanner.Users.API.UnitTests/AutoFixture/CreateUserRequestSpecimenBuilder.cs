using AutoFixture;
using AutoFixture.Kernel;
using Morsley.UK.YearPlanner.Users.API.Models.v1.Request;
using Morsley.UK.YearPlanner.Users.Domain.Enumerations;
using Morsley.UK.YearPlanner.Users.Domain.Models;
using System;

namespace Morsley.UK.YearPlanner.Users.API.UnitTests.AutoFixture
{
    public class CreateUserRequestSpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (context == null) throw new ArgumentNullException(nameof(context));

            var seededRequest = request as SeededRequest;

            if (seededRequest == null) return new NoSpecimen();

            var type = seededRequest.Request as Type;

            if (type == null) return new NoSpecimen();

            if (type != typeof(CreateUserRequest)) return new NoSpecimen();

            var title = context.Create<Title>();
            var firstName = nameof(User.FirstName) + "___" + context.Create<string>();
            var lastName = nameof(User.LastName) + "___" + context.Create<string>();
            var sex = context.Create<Sex>();

            var createUserRequest = new CreateUserRequest
            {
                Title = title.ToString(),
                FirstName = firstName,
                LastName = lastName,
                Sex = sex.ToString()
            };

            return createUserRequest;
        }

        // ToDo
        // Randomly select a title, but based upon a sex (if available).
        // Randomly select a sex, but based upon a title (if available).
    }
}