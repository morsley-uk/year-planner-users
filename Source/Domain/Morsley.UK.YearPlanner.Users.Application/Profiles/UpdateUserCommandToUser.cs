using AutoMapper;

namespace Morsley.UK.YearPlanner.Users.Application.Profiles
{
    public class UpdateUserCommandToUser : Profile
    {
        public UpdateUserCommandToUser()
        {
            CreateMap<Application.Commands.UpdateUserCommand, Domain.Models.User>();
        }
    }
}