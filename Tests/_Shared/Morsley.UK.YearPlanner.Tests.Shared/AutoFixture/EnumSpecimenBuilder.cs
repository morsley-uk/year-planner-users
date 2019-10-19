using AutoFixture.Kernel;
using System;
using System.Linq;

namespace Morsley.UK.YearPlanner.Users.Tests.Shared.AutoFixture
{
    public class EnumSpecimenBuilder<T> : ISpecimenBuilder where T: struct
    {
        private static Random _random = new Random();
        private T[] _enumValues;

        public EnumSpecimenBuilder()
        {
            if (!typeof(T).IsEnum) throw new ArgumentException("T must be an enum!");
            _enumValues = Enum.GetValues(typeof(T)).Cast<T>().ToArray();
        }

        public object Create(object request, ISpecimenContext context)
        {
            var t = request as Type;

            if (t == null || t != typeof(T)) return new NoSpecimen();

            var index = _random.Next(0, _enumValues.Length - 1);
            return _enumValues.GetValue(index);
        }
    }
}