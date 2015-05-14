using System;
using System.Text;

namespace Hqub.Mellody.Web
{
    public static class Logger
    {
        private static readonly NLog.Logger _instance;
        public static NLog.Logger Instance { get { return _instance; } }

        static Logger()
        {
            _instance = NLog.LogManager.GetLogger("Main");
        }


        /// <summary>
        /// is writing message about error into journal, as well as stacktrace.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        public static void AddException(string message, Exception exception)
        {
            _instance.Error("{0}\n{1}", message, exception.StackTrace);
        }

        /// <summary>
        /// is writing message about error into journal. Used standard NLog method 'ErrorException'.
        /// </summary>
        /// <param name="exception">Instance of the exception.</param>
        public static void AddException(Exception exception)
        {
            if(exception == null)
                return;

            var error = new StringBuilder();
            error.AppendFormat("{0} \n\n {1}", exception.Message, exception.StackTrace);

            _instance.ErrorException(exception.Message, exception);

            AddException(exception.InnerException);
        }

        /// <summary>
        /// is writing message about error, stacktrace and inner exception.
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="exception">Instance of the exception</param>
        public static void AddExceptionFull(string message, Exception exception)
        {
            _instance.Error(message);

            AddException(exception);
        }

        /// <summary>
        /// logged app launch.
        /// </summary>
        public static void LogApplicationStart()
        {
            Instance.Trace("Application run success");
        }

        /// <summary>
        /// logged app end.
        /// </summary>
        public static void LogApplicationEnd()
        {
            Instance.Trace("Application end.");
        }

        private static string GetStarts(int count)
        {
            var builder = new StringBuilder();

            for (int i = 0; i < count; i++)
            {
                builder.Append("*");
            }

            return builder.ToString();
        }
    }
}