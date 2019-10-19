namespace Morsley.UK.YearPlanner.Users.Domain.Interfaces
{
    public interface IEnvironmentService
    {
        string GetVariable(string key);

        void SetVariable(string key, string value);
    }
}