using System;
using System.Collections.Generic;
using System.Text;

namespace Derivas.Exception
{
    /// <summary>
    /// Base exception class for the project
    /// </summary>
    public class DvBaseException : System.Exception
    {
        public DvBaseException(string msg) : base(msg)
        {
        }
    }

}
