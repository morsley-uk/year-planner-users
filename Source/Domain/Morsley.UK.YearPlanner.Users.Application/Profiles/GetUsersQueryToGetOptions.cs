using AutoMapper;

namespace Morsley.UK.YearPlanner.Users.Application.Profiles
{
    public class GetUsersQueryToGetOptions : Profile
    {
        public GetUsersQueryToGetOptions()
        {
            CreateMap<Application.Queries.GetUsersQuery,
                      Application.Models.GetOptions>();
        }
    }
}