using AutoMapper;
using Morsley.UK.YearPlanner.Users.Domain.Enumerations;
using System;

namespace Morsley.UK.YearPlanner.Users.API.Converters
{
    public class StringTitleToEnumTitleConverter : ITypeConverter<string?, Domain.Enumerations.Title?>
    {
        public Title? Convert(string? source, Title? destination, ResolutionContext context)
        {
            if (source == null) return null;
            return (Title)Enum.Parse(typeof(Title), source, true);
        }
    }
}