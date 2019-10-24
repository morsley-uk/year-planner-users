namespace Morsley.UK.YearPlanner.Users.API.Models.v1.Request
{
    public class CreateUserRequest
    {
        public string? Title { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? Sex { get; set; }
    }
}