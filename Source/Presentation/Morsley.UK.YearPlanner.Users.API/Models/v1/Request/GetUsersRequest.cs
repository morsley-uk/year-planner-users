namespace Morsley.UK.YearPlanner.Users.API.Models.v1.Request
{
    public class GetUsersRequest
    {
        public int? PageNumber { get; set; }

        public int? PageSize { get; set; }

        public string? SearchQuery { get; set; }
    }
}