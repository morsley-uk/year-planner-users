using Morsley.UK.YearPlanner.Users.Domain.Enumerations;

namespace Morsley.UK.YearPlanner.Users.Domain.Interfaces
{
    public interface IOrdering
    {
        string Key { get; }

        SortOrder Order { get; }
    }
}