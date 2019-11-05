using System;

namespace Morsley.UK.YearPlanner.Users.API.Models.v1.Request
{
    public class PartiallyUpsertUserRequest
    {
        private string? _title;
        private string? _firstName;
        private string? _lastName;
        private string? _sex;

        public PartiallyUpsertUserRequest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; private set; }

        public string? Title
        {
            get => _title;
            set
            {
                TitleChanged = true;
                _title = value;
            }
        }

        public bool TitleChanged { get; private set; }

        public string? FirstName
        {
            get => _firstName;
            set
            {
                FirstNameChanged = true;
                _firstName = value;
            }
        }

        public bool FirstNameChanged { get; private set; }

        public string? LastName
        {
            get => _lastName;
            set
            {
                LastNameChanged = true;
                _lastName = value;
            }
        }

        public bool LastNameChanged { get; private set; }

        public string? Sex
        {
            get => _sex;
            set
            {
                SexChanged = true;
                _sex = value;
            }
        }

        public bool SexChanged { get; private set; }
    }
}