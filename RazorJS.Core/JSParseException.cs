using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RazorJS
{
    public class JSFileParserException : Exception
    {
        public JSFileParserException(string message) : base(message) { }
        public JSFileParserException(string message, Exception innerException) : base(message, innerException) { }
    }
}
