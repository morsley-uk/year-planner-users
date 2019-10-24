using System;
using Microsoft.AspNetCore.JsonPatch;

namespace Morsley.UK.YearPlanner.Users.API.Models.v1.Request
{
    public class PartiallyUpdateUserRequest : JsonPatchDocument<UpdateUserRequest>
    {
        public Guid Id { get; set; }
    }
}