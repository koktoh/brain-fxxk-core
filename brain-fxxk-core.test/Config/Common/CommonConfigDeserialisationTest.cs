using BFCore.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace brain_fxxk_core.test.Config.Common
{
    [TestClass]
    public class CommonConfigDeserialisationTest
    {
        [TestMethod]
        public void DeserializeCorrectlyTest()
        {
            var jsonPath = @"TestData\CommonConfigJsons\Normal.json";

            var expected = new CommonConfig
            {
                MemorySize = 1000,
                EnableCommentOut = false,
            };

            var result = ConfigManager.Import<CommonConfig>(jsonPath);

            result.IsStructuralEqual(expected);

        }


        [TestMethod]
        public void WithoutCommentConfigDeserializeCorrectlyTest()
        {
            var jsonPath = @"TestData\CommonConfigJsons\WithoutCommentConfig.json";

            var expected = new CommonConfig
            {
                MemorySize = 1000,
                EnableCommentOut = true,
            };

            var result = ConfigManager.Import<CommonConfig>(jsonPath);

            result.IsStructuralEqual(expected);

        }
    }
}
