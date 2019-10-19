using AutoFixture.Kernel;
using Morsley.UK.YearPlanner.Users.Domain.Enumerations;
using System;
using System.Reflection;

namespace Morsley.UK.YearPlanner.Users.Tests.Shared.AutoFixture
{
    public class SexSpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            var pi = request as PropertyInfo;

            if (pi == null) return new NoSpecimen();

            if (pi.PropertyType != typeof(string) ||
                !pi.Name.Contains("Sex")) return new NoSpecimen();

            var random = new Random();
            var randomNumber = random.Next(1, 3);

            switch (randomNumber)
            {
                case 1: return Sex.Male;
                case 2: return Sex.Female;
                default: return null;
            }
        }
    }
}