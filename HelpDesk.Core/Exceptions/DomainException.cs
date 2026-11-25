using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDesk.Core.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message) { }
    }
}
