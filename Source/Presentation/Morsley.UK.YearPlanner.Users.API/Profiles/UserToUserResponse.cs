using AutoMapper;

namespace Morsley.UK.YearPlanner.Users.API.Profiles
{
    public class UserToUserResponse : Profile
    {
        public UserToUserResponse()
        {
            CreateMap<Domain.Models.User, API.Models.v1.Response.UserResponse>();
        }
    }
}