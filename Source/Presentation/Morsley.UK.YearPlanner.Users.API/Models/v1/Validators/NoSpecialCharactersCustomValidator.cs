using FluentValidation;

namespace Morsley.UK.YearPlanner.Users.API.Models.v1.Validators
{
    public static class NoSpecialCharactersCustomValidator
    {
        public static IRuleBuilderOptions<T, string> NoBlacklistedCharacters<T>(
            this IRuleBuilder<T, string> ruleBuilder, string blacklistedCharacters)
        {
            //return ruleBuilder.Custom((element, context) =>
            //{
            //    context.AddFailure("Blah!");
            //});
            return ruleBuilder.SetValidator(new BlacklistedCharactersValidator(blacklistedCharacters));
        }
    }
}
