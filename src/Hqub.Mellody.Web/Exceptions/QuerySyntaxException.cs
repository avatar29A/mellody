using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hqub.Mellody.Web.Exceptions
{
    /// <summary>
    /// Exception thrown if instanse of QueryEntity is unsupported.
    /// </summary>
    public class QuerySyntaxException : Exception
    {
        public QuerySyntaxException()
        {
            
        }

        public QuerySyntaxException(string message) : base(message)
        {
            
        }
    }
}