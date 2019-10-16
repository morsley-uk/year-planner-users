using System;

namespace Morsley.UK.YearPlanner.Users.API.Models
{
    public class UserResponse
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Sex { get; set; }
    }
}