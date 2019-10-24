using System;

namespace Morsley.UK.YearPlanner.Users.Application.Extensions
{
    public static class StringExtensions
    {
        public static TEnum? ToEnum<TEnum>(this string value) where TEnum : struct
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            return (TEnum)Enum.Parse<TEnum>(value);
        }
    }
}