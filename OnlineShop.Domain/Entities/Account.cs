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

        public Account(Guid id, string name, string email, string password)
        {
            if(string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            if(string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(email));
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(password));
            Id = id;
            _name = name;
            _email = email;
            _hashedPassword = password;
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
        public string Password
        {
            get=> _hashedPassword;
            set
            {
                if (IsValidPassword(value))
                {
                    _hashedPassword = value;
                }
                else
                {
                    throw new ArgumentException("Invalid password.");
                }
            }
        }
        private bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length <8)
            {
                return false;
            }
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z]).{8,}$";
            if (!Regex.IsMatch(password, pattern))
            {
                return false;
            }
            return true;
        }
    }
}
