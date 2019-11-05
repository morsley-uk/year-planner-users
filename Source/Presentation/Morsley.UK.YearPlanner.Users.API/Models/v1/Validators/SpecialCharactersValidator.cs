using FluentValidation.Validators;

namespace Morsley.UK.YearPlanner.Users.API.Models.v1.Validators
{
    public class BlacklistedCharactersValidator : PropertyValidator
    {
        private string _blacklistedCharacters;

        public BlacklistedCharactersValidator(string blacklistedCharacters) : base("Contains blacklisted characters!")
        {
            _blacklistedCharacters = blacklistedCharacters;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var value = context.PropertyValue as string;

            if (string.IsNullOrWhiteSpace(value)) return true;

            foreach (var character in value)
            {
                if (_blacklistedCharacters.Contains(character)) return false;
            }

            return true;
        }
    }
}