using AutoMapper;
using Morsley.UK.YearPlanner.Users.Domain.Enumerations;
using System;

namespace Morsley.UK.YearPlanner.Users.API.Converters
{
    public class StringSexToEnumSexConverter : ITypeConverter<string?, Domain.Enumerations.Sex?>
    {
        public Sex? Convert(string? source, Sex? destination, ResolutionContext context)
        {
            if (source == null) return null;
            return (Sex)Enum.Parse(typeof(Sex), source, true);
        }
    }
}