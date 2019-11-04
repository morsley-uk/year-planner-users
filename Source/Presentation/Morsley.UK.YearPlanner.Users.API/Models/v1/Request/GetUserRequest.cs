using System;

namespace Morsley.UK.YearPlanner.Users.API.Models.v1.Request
{
    public class GetUserRequest
    {
        public GetUserRequest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }
    }
}