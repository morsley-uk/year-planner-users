using AutoMapper;

namespace Morsley.UK.YearPlanner.Users.Application.Profiles
{
    public class AddUserCommandToUser : Profile
    {
        public AddUserCommandToUser()
        {
            CreateMap<Application.Commands.AddUserCommand, Domain.Models.User>();
        }
    }
}