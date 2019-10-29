using AutoMapper;

namespace Morsley.UK.YearPlanner.Users.API.Profiles
{
    public class GetUsersRequestToGetUsersQuery : Profile
    {
        public GetUsersRequestToGetUsersQuery()
        {
            CreateMap<API.Models.v1.Request.GetUsersRequest, Application.Queries.GetUsersQuery>();
        }
    }
}