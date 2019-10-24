using Morsley.UK.YearPlanner.Users.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Morsley.UK.YearPlanner.Users.Domain.Models
{
    public class User : Entity<Guid>
    {
        private string _firstName;
        private string _lastName;

        private IList<Address> _addresses;
        private IList<Email> _emails;
        private IList<Phone> _phones;

        public User(string firstName, string lastName)
        {
            SetFirstName(firstName);
            SetLastName(lastName);

            _addresses = new List<Address>();
            _emails = new List<Email>();
            _phones = new List<Phone>();
        }

        public Title? Title { get; set; }

        public string FirstName
        {
            get => _firstName;
            set => SetFirstName(value);
        }

        public string LastName
        {
            get => _lastName;
            set => SetLastName(value);
        }

        public Sex? Sex { get; set; }

        public IReadOnlyList<Email> Emails => _emails.ToList();

        public IReadOnlyList<Phone> Phones => _phones.ToList();

        public IReadOnlyList<Address> Addresses => _addresses.ToList();

        public void AddAddress(Address newAddress)
        {
            // Make sure only one address is primary...
            // ToDo

            _addresses.Add(newAddress);
        }

        public void AddEmail(Email newEmail)
        {
            // Make sure only one email is primary...
            // ToDo

            _emails.Add(newEmail);
        }

        public void AddPhone(Phone newPhone)
        {
            // Make sure only one phone is primary...
            // ToDo

            _phones.Add(newPhone);
        }

        private void SetFirstName(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("Cannot be null or empty!", nameof(firstName));
            _firstName = firstName;
        }

        private void SetLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Cannot be null or empty!", nameof(lastName));
            _lastName = lastName;
        }
    }
}