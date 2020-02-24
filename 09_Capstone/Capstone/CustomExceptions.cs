using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public class InvalidDateRangeSelectionException : Exception
    {
        public InvalidDateRangeSelectionException(string message) : base(message)
        {
        }
    }

    public class InvalidDateScopeException : Exception
    {
        public InvalidDateScopeException(string message) : base(message)
        {
        }
    }

}
