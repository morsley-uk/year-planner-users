using AutoMapper;
using Morsley.UK.YearPlanner.Users.API.Converters;

namespace Morsley.UK.YearPlanner.Users.API.Profiles
{
    public class CreateUserRequestToAddUserCommand : Profile
    {
        public CreateUserRequestToAddUserCommand()
        {
            CreateMap<string?, Domain.Enumerations.Sex?>()
                .ConvertUsing(new StringSexToEnumSexConverter());

            CreateMap<string?, Domain.Enumerations.Title?>()
                .ConvertUsing(new StringTitleToEnumTitleConverter());

            CreateMap<API.Models.v1.Request.CreateUserRequest, Application.Commands.AddUserCommand>();
        }
    }
}