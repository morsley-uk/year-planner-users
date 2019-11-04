using System;

namespace Morsley.UK.YearPlanner.Users.API.Models.v1.Request
{
    public class DeleteUserRequest
    {
        public DeleteUserRequest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }
    }
}