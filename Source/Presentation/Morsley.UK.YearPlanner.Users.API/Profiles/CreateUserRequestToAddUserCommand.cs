using AutoMapper;
using Morsley.UK.YearPlanner.Users.Domain.Enumerations;
using System;

namespace Morsley.UK.YearPlanner.Users.API.Profiles
{
    public class CreateUserRequestToAddUserCommand : Profile
    {
        public CreateUserRequestToAddUserCommand()
        {
            CreateMap<string?, Domain.Enumerations.Sex?>()
                .ConvertUsing(new StringSexToEnumSexConvertor());

            CreateMap<string?, Domain.Enumerations.Title?>()
                .ConvertUsing(new StringTitleToEnumTitleConvertor());

            CreateMap<API.Models.v1.Request.CreateUserRequest,
                      Application.Commands.AddUserCommand>();
        }
    }

    public class StringSexToEnumSexConvertor : ITypeConverter<string?, Domain.Enumerations.Sex?>
    {
        public Sex? Convert(string source, Sex? destination, ResolutionContext context)
        {
            if (source == null) return null;
            return (Sex)Enum.Parse(typeof(Sex), source, true);
        }
    }

    public class StringTitleToEnumTitleConvertor : ITypeConverter<string?, Domain.Enumerations.Title?>
    {
        public Title? Convert(string source, Title? destination, ResolutionContext context)
        {
            if (source == null) return null;
            return (Title)Enum.Parse(typeof(Title), source, true);
        }
    }

}