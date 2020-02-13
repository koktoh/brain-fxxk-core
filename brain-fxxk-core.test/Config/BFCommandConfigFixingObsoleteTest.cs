using System.IO;
using BFCore.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace brain_fxxk_core.test.Config
{
    [TestClass]
    public class BFCommandConfigFixingObsoleteTest
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
        public void FixingObsoleteTest()
        {
            var obsoleteJsonPath = @"TestData\BFCommandConfigJsons\Obsolete.json";

            var expectedFile = @"TestData\BFCommandConfigJsons\ExpectedObsolete.json";

            var obsoleteConfig = ConfigManager.Import<BFCommandConfig>(obsoleteJsonPath);

            ConfigManager.Save(obsoleteConfig, this._destPath);

            FileAssert.AreEqual(expectedFile, this._destPath);
        }
    }
}
