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


        public static void AddException(string message, Exception exception)
        {
            _instance.Error("{0}\n{1}", message, exception.StackTrace);
        }

        public static void AddException(Exception exception)
        {
            var stars = GetStarts(10);
            var error = new StringBuilder();
            error.AppendLine(stars);
            error.AppendFormat("{0} \n\n {1}", exception.Message, exception.StackTrace);

            if (exception.InnerException != null)
            {
                error.AppendLine("\n[Inner exception]");
                error.AppendFormat("{0} \n\n {1}", exception.InnerException.Message, exception.InnerException.StackTrace);
            }

            error.AppendLine(stars);

            System.Diagnostics.Debug.WriteLine("[{0}] {1}", DateTime.Now, error);
            _instance.ErrorException(exception.Message, exception);
        }

        public static void AddExceptionFull(string message, Exception exception)
        {
            var error = new StringBuilder();

            error.AppendLine(message);
            error.AppendFormat("{0} \n\n {1}", exception.Message, exception.StackTrace);

            if (exception.InnerException != null)
            {
                error.AppendLine("\n[Inner exception]");
                error.AppendFormat("{0} \n\n {1}", exception.InnerException.Message, exception.InnerException.StackTrace);
            }

            _instance.Error(error.ToString());
        }

        public static void LogApplicationStart()
        {
            Instance.Trace("Application run success");
        }

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