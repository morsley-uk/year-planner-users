using System;
using Morsley.UK.YearPlanner.Users.Domain.Interfaces;

namespace Morsley.UK.YearPlanner.Users.Domain.Models
{
    public class Phone : Entity<Guid>
    {
        public string Value { get; set; }

        public bool IsValidated { get; set; }

        public bool IsPrimary { get; set; }
    }
}