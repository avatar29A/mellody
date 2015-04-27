using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Mellody.Core.Utilities
{
    public class Cache
    {
        #region .ctor

        private Cache()
        {

        }

        #endregion

        #region Singleton

        private static Cache _instance;
        public static Cache Instance
        {
            get { return _instance = _instance ?? new Cache(); }
        }

        #endregion
    }
}
