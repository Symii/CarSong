using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracaDomowaCS.Exceptions
{
    internal class IdNotFoundException : Exception
    {
        public IdNotFoundException() : base("Nie znaleziono dostepnego id dla tej tabeli")
        {
        }

        public IdNotFoundException(string message) : base(message)
        {
        }

        public IdNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
