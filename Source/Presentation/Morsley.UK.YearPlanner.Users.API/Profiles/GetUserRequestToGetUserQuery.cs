using AutoMapper;

namespace Morsley.UK.YearPlanner.Users.API.Profiles
{
    public class GetUserRequestToGetUserQuery : Profile
    {
        public GetUserRequestToGetUserQuery()
        {
            CreateMap<API.Models.v1.Request.GetUserRequest, Application.Queries.GetUserQuery>();
        }
    }
}