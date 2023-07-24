using OnlineShop.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace OnlineShop.Domain
{
    public class Account : IEntity
    {
        private string _name;
        private string _email;
        private string _hashedPassword;

        public Account(string name, string email, string hashedPassword)
        {
            if(string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            if(string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(email));
            if (string.IsNullOrWhiteSpace(hashedPassword))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(hashedPassword));
            Id = Guid.NewGuid();
            _name = name;
            _email = email;
            _hashedPassword = hashedPassword;
        }

        public Guid Id { get; init; }
        public string Name
        { 
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));
                _name = value;
            }
        }
        public string Email
        {
            get => _email;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));
                }
                if (!new EmailAddressAttribute().IsValid(value))
                {
                    throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));
                }
                _email = value;
            }
        }
        public string HashedPassword
        {
            get=> _hashedPassword;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));
                _hashedPassword = value;
            }
        }
    }
}
