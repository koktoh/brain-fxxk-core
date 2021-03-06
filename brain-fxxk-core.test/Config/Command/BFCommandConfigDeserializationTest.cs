﻿using BFCore.Command;
using BFCore.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace brain_fxxk_core.test.Config.Command
{
    [TestClass]
    public class BFCommandConfigDeserializationTest
    {
        [TestMethod]
        public void DeserializeCorrectlyTest()
        {
            var jsonPath = @"TestData\BFCommandConfigJsons\Normal.json";

            var expected = new BFCommandConfig
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

            var result = ConfigManager.Import<BFCommandConfig>(jsonPath);

            result.IsStructuralEqual(expected);
        }

        [TestMethod]
        public void DeserializeCorrectlyObsoleteTest()
        {
            var jsonPath = @"TestData\BFCommandConfigJsons\Obsolete.json";

            var expected = new BFCommandConfig
            {
                Increment = new BFCommand("inc", BFCommandType.Increment),
                Decrement = new BFCommand("dec", BFCommandType.Decrement),
                MoveRight = new BFCommand("moveR", BFCommandType.MoveRight),
                MoveLeft = new BFCommand("moveL", BFCommandType.MoveLeft),
                LoopHead = new BFCommand("roopH", BFCommandType.LoopHead),
                LoopTail = new BFCommand("roopT", BFCommandType.LoopTail),
                Read = new BFCommand("read", BFCommandType.Read),
                Write = new BFCommand("write", BFCommandType.Write),
                BeginComment = new BFCommand("beginC", BFCommandType.BeginComment),
                EndComment = new BFCommand("endC", BFCommandType.EndComment)
            };

            var result = ConfigManager.Import<BFCommandConfig>(jsonPath);

            result.IsStructuralEqual(expected);
        }
    }
}
