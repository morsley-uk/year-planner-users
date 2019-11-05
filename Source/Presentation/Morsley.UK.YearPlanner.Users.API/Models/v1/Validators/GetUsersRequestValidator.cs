using FluentValidation;
using Morsley.UK.YearPlanner.Users.API.Models.v1.Request;

namespace Morsley.UK.YearPlanner.Users.API.Models.v1.Validators
{
    public class GetUsersRequestValidator : AbstractValidator<GetUsersRequest>
    {
        public GetUsersRequestValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0);

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .LessThanOrEqualTo(25);
        }
    }
}