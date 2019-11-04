namespace Morsley.UK.YearPlanner.Users.API.Models.v1.Request
{
    public class GetUsersRequest
    {
        private uint _pageNumber = 1;
        private uint _pageSize = 10;

        public uint PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value <= 0 ? 1 : value;
        }

        public uint PageSize
        {
            get => _pageSize;
            set => _pageSize = value <= 0 ? 1 : value;
        }

        public string? SearchQuery { get; set; }
    }
}