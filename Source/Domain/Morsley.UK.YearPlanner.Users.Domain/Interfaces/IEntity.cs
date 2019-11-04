using System;

namespace Morsley.UK.YearPlanner.Users.Domain.Interfaces
{
    public interface IEntity<T> : IEquatable<T>
    {
        T Id { get; }

        DateTime Created { get; set; }

        DateTime? Updated { get; set; }
    }
}