using AutoMapper;
using Morsley.UK.YearPlanner.Users.API.Converters;

namespace Morsley.UK.YearPlanner.Users.API.Profiles
{
    public class UpdateUserRequestToUpdateUserCommand : Profile
    {
        public UpdateUserRequestToUpdateUserCommand()
        {
            CreateMap<string?, Domain.Enumerations.Sex?>()
                .ConvertUsing(new StringSexToEnumSexConverter());

            CreateMap<string?, Domain.Enumerations.Title?>()
                .ConvertUsing(new StringTitleToEnumTitleConverter());

            CreateMap<API.Models.v1.Request.UpsertUserRequest, Application.Commands.UpdateUserCommand>();
        }
    }
}