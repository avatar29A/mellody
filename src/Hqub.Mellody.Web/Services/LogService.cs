using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hqub.Mellody.Web.Services
{
    public class LogService : Music.Services.ILogService
    {
        public void AddException(string message, Exception exception)
        {
            Logger.AddException(message, exception);
        }

        public void AddException(Exception exception)
        {
            Logger.AddException(exception);
        }

        public void AddExceptionFull(string message, Exception exception)
        {
            Logger.AddExceptionFull(message, exception);
        }

        public void LogApplicationStart()
        {
            Logger.LogApplicationStart();
        }

        public void LogApplicationEnd()
        {
            Logger.LogApplicationEnd();
        }
    }
}