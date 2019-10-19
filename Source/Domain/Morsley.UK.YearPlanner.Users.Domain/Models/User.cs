using Morsley.UK.YearPlanner.Users.Domain.Enumerations;
using Morsley.UK.YearPlanner.Users.Domain.Exceptions;
using System;
using System.Collections.Generic;

namespace Morsley.UK.YearPlanner.Users.Domain.Models
{
    public class User : Entity<Guid>
    {
        public User()
        {
            Addresses = new List<Address>();
            Emails = new List<Email>();
            Phones = new List<Phone>();
        }

        private string _firstName;
        private string _lastName;

        public Title Title { get; set; }

        public string FirstName
        {
            get => _firstName;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new DomainModelException();
                _firstName = value;
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                if (string.IsNullOrWhiteSpace(value)) throw new DomainModelException();
                _lastName = value;
            }
        }

        public Sex? Sex { get; set; }

        public IList<Email> Emails { get; protected set; }

        public IList<Phone> Phones { get; protected set; }

        public IList<Address> Addresses { get; protected set; }

        public void AddAddress(Address newAddress)
        {
            // Make sure only one address is primary...
            // ToDo

            Addresses.Add(newAddress);
        }

        public void AddEmail(Email newEmail)
        {
            // Make sure only one email is primary...
            // ToDo

            Emails.Add(newEmail);
        }

        public void AddPhone(Phone newPhone)
        {
            // Make sure only one phone is primary...
            // ToDo

            Phones.Add(newPhone);
        }
    }
}