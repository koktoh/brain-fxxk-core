using System.IO;
using BFCore.Command;
using BFCore.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace brain_fxxk_core.test.Config
{
    [TestClass]
    public class BFCommandConfigDeserializationTest
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
        public void DeerializeCorrectlyTest()
        {
            var src = new BFCommandConfig
            {
                Increment = new BFCommand("inc", BFCommandType.Increment),
                Decrement = new BFCommand("dec", BFCommandType.Decrement),
                MoveRight = new BFCommand("moveR", BFCommandType.MoveRight),
                MoveLeft = new BFCommand("moveL", BFCommandType.MoveLeft),
                LoopHead = new BFCommand("loopH", BFCommandType.LoopHead),
                LoopTail = new BFCommand("loopT", BFCommandType.LoopTail),
                Read = new BFCommand("read", BFCommandType.Read),
                Write = new BFCommand("write", BFCommandType.Write),
                BeginComment = new BFCommand("beginC", BFCommandType.BeginComment),
                EndComment = new BFCommand("endC", BFCommandType.EndComment)
            };

            var expectedFile = @"TestData\BFCommandConfigJsons\Expected.json";

            ConfigManager.Save(src, this._destPath);

            FileAssert.AreEqual(expectedFile, this._destPath);
        }
    }
}
