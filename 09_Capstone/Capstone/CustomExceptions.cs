using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone
{
    public class InvalidDateSelectionException : Exception
    {
        public InvalidDateSelectionException(string message) : base(message)
        {
        }
    }

}
