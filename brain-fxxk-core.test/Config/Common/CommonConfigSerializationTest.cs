using System.IO;
using BFCore.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace brain_fxxk_core.test.Config.Common
{
    [TestClass]
    public class CommonConfigSerializationTest
    {
        private readonly string _destPath = @"TestData\Result.json";

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(this._destPath))
            {
                File.Delete(this._destPath);
            }
        }

        [TestMethod]
        public void SerializeCorrectlyTest()
        {
            var src = new CommonConfig
            {
                MemorySize = 2000,
                EnableCommentOut = false,
            };

            var expectedFile = @"TestData\CommonConfigJsons\Expected.json";

            ConfigManager.Save(src, this._destPath);

            FileAssert.AreEqual(expectedFile, this._destPath);
        }
    }
}
