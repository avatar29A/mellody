using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Mellody.Music.Services.Exceptions
{
    public class EmptySearchResultException : Exception
    {
        public EmptySearchResultException() : base("Search result is empty.")
        {
        }
    }
}
