using AutoMapper;

namespace Morsley.UK.YearPlanner.Users.API.Profiles
{
    public class DeleteUserRequestToDeleteUserCommand : Profile
    {
        public DeleteUserRequestToDeleteUserCommand()
        {
            CreateMap<API.Models.v1.Request.DeleteUserRequest, Application.Commands.DeleteUserCommand>();
        }
    }
}