using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Domain.Exceptions
{
    public class InvalidCodeException : DomainException
    {
        public InvalidCodeException(string message) : base(message)
        {

        }
    }
}
