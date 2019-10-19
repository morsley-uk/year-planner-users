using Morsley.UK.YearPlanner.Users.Domain.Interfaces;
using System;

namespace Morsley.UK.YearPlanner.Users.Persistence.Models
{
    public class Filter : IFilter
    {
        public Filter(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Cannot be null or empty!", nameof(key));
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Cannot be null or empty!", nameof(value));

            Key = key;
            Value = value;
        }

        public string Key { get; }

        public string Value { get; }
    }
}