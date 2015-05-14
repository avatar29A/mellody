using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hqub.Mellody.Music.Services
{
    public interface ILogService
    {
        void AddException(string message, Exception exception);
        void AddException(Exception exception);
        void AddExceptionFull(string message, Exception exception);

        void LogApplicationStart();
        void LogApplicationEnd();

    }
}
