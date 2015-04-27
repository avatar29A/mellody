using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting;

namespace Hqub.Mellody.Music.Utilities
{
    using System.Diagnostics;

    public static class PythonInvoker
    {
        public static string Execute(string pythonPath, List<string> args)
        {
            var processStartInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                FileName = pythonPath,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                Arguments = string.Join(" ", args)
            };

            using (var process = Process.Start(processStartInfo))
            {
                if (process == null)
                    return string.Empty;

                using (var reader = process.StandardOutput)
                {
                    var result = reader.ReadToEnd();
                    return result.Trim();
                }
            }
        }
    }
}
