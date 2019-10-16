using System.ComponentModel.DataAnnotations;

namespace Morsley.UK.YearPlanner.Users.API.Models
{
    public class CreateUserRequest
    {
        public string Title { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Sex { get; set; }
    }
}