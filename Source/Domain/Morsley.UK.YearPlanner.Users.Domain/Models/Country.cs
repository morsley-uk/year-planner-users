using System;
using Morsley.UK.YearPlanner.Users.Domain.Interfaces;

namespace Morsley.UK.YearPlanner.Users.Domain.Models
{
    public class Country : Entity<Guid>
    {
        public string Value { get; set; }

        public string TwoLetterCode { get; set; }

        public string ThreeLetterCode { get; set; }
    }
}