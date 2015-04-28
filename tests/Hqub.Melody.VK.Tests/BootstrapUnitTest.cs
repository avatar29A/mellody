using System;
using System.Collections.Generic;
using Hqub.Mellody.Music.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hqub.Melody.Music.Tests.Tests
{
    [TestClass]
    public class BootstrapUnitTest
    {
        [TestMethod]
        public void CheckPythonInstalled()
        {
            const string scriptName = @"Scripts\VKAuth\fetch_token.py";

            var result = PythonInvoker.Execute("python.exe",  new List<string>
            {
                scriptName,
                string.Empty,
                string.Empty,
                string.Empty,
                string.Empty
            });

            Assert.AreEqual(result, string.Empty);
        }
    }
}
