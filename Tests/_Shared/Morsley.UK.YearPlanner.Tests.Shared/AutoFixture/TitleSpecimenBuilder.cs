using AutoFixture.Kernel;
using Morsley.UK.YearPlanner.Users.Domain.Enumerations;
using System;
using System.Reflection;

namespace Morsley.UK.YearPlanner.Users.Tests.Shared.AutoFixture
{
    public class TitleSpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            var pi = request as PropertyInfo;

            if (pi == null) return new NoSpecimen();

            if (pi.PropertyType != typeof(string) ||
                !pi.Name.Contains("Title")) return new NoSpecimen();

            var random = new Random();
            var randomNumber = random.Next(1, 7);

            switch (randomNumber)
            {
                case 1: return Title.Mr;
                case 2: return Title.Master;
                
                case 3: return Title.Ms;
                case 4: return Title.Mrs;
                case 5: return Title.Miss;
                
                case 6: return Title.Mx;
                
                default: return null;
            }
        }
    }
}