using System;
using System.IO;
using System.Collections.Generic;
using NUnit.Framework;
using XboxWebApi.Common;
using XboxWebApi.Authentication;
using XboxWebApi.Authentication.Model;

namespace XboxWebApi.UnitTests
{
    public class TestDataProvider
    {
        public Dictionary<string, string> TestData { get; internal set; }
        public TestDataProvider(string path)
        {
            TestData = new Dictionary<string, string>();
            string rootPath = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(
                TestContext.CurrentContext.TestDirectory)));
            string directoryPath = Path.Combine(rootPath, "TestData", path);
            string[] files = Directory.GetFiles(directoryPath);
            foreach (string file in files)
            {
                TestData.Add(Path.GetFileName(file), File.ReadAllText(file));
            }
        }
    }
}