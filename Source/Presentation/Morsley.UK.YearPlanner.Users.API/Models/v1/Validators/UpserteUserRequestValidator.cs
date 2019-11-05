using FluentValidation;
using Microsoft.Extensions.Configuration;
using Morsley.UK.YearPlanner.Users.API.Models.v1.Request;
using Morsley.UK.YearPlanner.Users.Domain.Enumerations;

namespace Morsley.UK.YearPlanner.Users.API.Models.v1.Validators
{
    public class UpsertUserRequestValidator : AbstractValidator<UpsertUserRequest>
    {
        public UpsertUserRequestValidator(IConfiguration configuration)
        {
            var blacklistedCharacters = configuration["BlacklistedCharacters"];

            RuleFor(x => x.Title)
                .IsEnumName(typeof(Title), caseSensitive: false);

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .Length(1, 255)
                .NoBlacklistedCharacters(blacklistedCharacters);

            RuleFor(x => x.LastName)
                .NotEmpty()
                .Length(1, 255)
                .NoBlacklistedCharacters(blacklistedCharacters);

            RuleFor(x => x.Sex)
                .IsEnumName(typeof(Sex), caseSensitive: false);
        }
    }
}