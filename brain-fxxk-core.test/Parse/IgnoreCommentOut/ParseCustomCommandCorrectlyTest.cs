using System.Collections.Generic;
using System.Linq;
using BFCore.Command;
using BFCore.Config;
using BFCore.Extesion;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace brain_fxxk_core.test.Parse.IgnoreCommentOut
{
    [TestClass]
    public class ParseCustomCommandCorrectlyTest : IgnoreCommentOutParserTestBase
    {
        private static readonly BFCommandConfig _commandConfig = new BFCommandConfig
        {
            Increment = new BFCommand("inc", BFCommandType.Increment),
            Decrement = new BFCommand("dec", BFCommandType.Decrement),
            MoveRight = new BFCommand("toR", BFCommandType.MoveRight),
            MoveLeft = new BFCommand("toL", BFCommandType.MoveLeft),
            LoopHead = new BFCommand("loopH", BFCommandType.LoopHead),
            LoopTail = new BFCommand("loopT", BFCommandType.LoopTail),
            Read = new BFCommand("read", BFCommandType.Read),
            Write = new BFCommand("write", BFCommandType.Write),
            BeginComment = new BFCommand("beginC", BFCommandType.BeginComment),
            EndComment = new BFCommand("endC", BFCommandType.EndComment),
        };

        protected override BFCommandConfig CommandConfig => _commandConfig;

        [TestMethod]
        [TestCase("")]
        public void EmptyTest()
        {
            this.TestContext.Run<string>(code =>
            {
                this._parser.Parse(code).Count().Is(0);
            });
        }

        [TestMethod]
        [TestCase(" ")]
        [TestCase("a")]
        [TestCase(" a")]
        [TestCase("a ")]
        [TestCase(" a ")]
        [TestCase("beginCa")]
        [TestCase("beginCaendC")]
        [TestCase(" beginCa")]
        [TestCase(" beginCaendC")]
        [TestCase(" beginCaendC ")]
        public void OnlyTriviaTest()
        {
            this.TestContext.Run<string>(code =>
            {
                var parsed = this._parser.Parse(code);
                parsed.All(x => !x.IsExecutable()).IsTrue($@"TetCase: ""{code}""");
            });
        }

        public static object[] ParseCorrectlyTestSource = new[]
        {
            new object[] { "inc", new[] { _commandConfig.Increment } },
            new object[] { "dec", new[] { _commandConfig.Decrement } },
            new object[] { "toR", new[] { _commandConfig.MoveRight } },
            new object[] { "toL", new[] { _commandConfig.MoveLeft } },
            new object[] { "loopH", new[] { _commandConfig.LoopHead } },
            new object[] { "loopT", new[] { _commandConfig.LoopTail } },
            new object[] { "read", new[] { _commandConfig.Read } },
            new object[] { "write", new[] { _commandConfig.Write } },
            new object[] { "beginC", new[] { new BFCommand("beginC", BFCommandType.Trivia) } },
            new object[] { "endC", new[] { new BFCommand("endC", BFCommandType.Trivia) } },
        };

        [TestMethod]
        [TestCaseSource(nameof(ParseCorrectlyTestSource))]
        public void ParseCorrectlyTest()
        {
            this.TestContext.Run<string, IEnumerable<BFCommand>>((code, expected) =>
            {
                var parsed = this._parser.Parse(code);
                parsed.Is(expected, $@"TestCase: ""{code}""");
            });
        }

        public static object[] ParseCorrectlyWithCommentTestSource = new[]
        {
            new object[] { "incinc", new[] { _commandConfig.Increment, new BFCommand(_commandConfig.Increment, 0, 3) } },
            new object[] { " incinc", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 1), new BFCommand(_commandConfig.Increment, 0, 4) } },
            new object[] { "inc inc", new[] { _commandConfig.Increment, new BFCommand(" ", BFCommandType.Trivia, 0, 3), new BFCommand(_commandConfig.Increment, 0, 4) } },
            new object[] { "incinc ", new[] { _commandConfig.Increment, new BFCommand(_commandConfig.Increment, 0, 3), new BFCommand(" ", BFCommandType.Trivia, 0, 6) } },
            new object[] { " inc inc", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 1), new BFCommand(" ", BFCommandType.Trivia, 0, 4), new BFCommand(_commandConfig.Increment, 0, 5) } },
            new object[] { "inc inc ", new[] { _commandConfig.Increment, new BFCommand(" ", BFCommandType.Trivia, 0, 3), new BFCommand(_commandConfig.Increment, 0, 4), new BFCommand(" ", BFCommandType.Trivia, 0, 7) } },
            new object[] { " incinc ", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 1), new BFCommand(_commandConfig.Increment, 0, 4), new BFCommand(" ", BFCommandType.Trivia, 0, 7) } },
            new object[] { " inc inc ", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 1), new BFCommand(" ", BFCommandType.Trivia, 0, 4), new BFCommand(_commandConfig.Increment, 0, 5), new BFCommand(" ", BFCommandType.Trivia, 0, 8) } },
            new object[] { "beginCaendCincinc", new[] { new BFCommand("beginCaendC", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 11), new BFCommand(_commandConfig.Increment, 0, 14) } },
            new object[] { "incbeginCaendCinc", new[] { _commandConfig.Increment, new BFCommand("beginCaendC", BFCommandType.Trivia, 0, 3), new BFCommand(_commandConfig.Increment, 0, 14) } },
            new object[] { "incincbeginCaendC", new[] { _commandConfig.Increment, new BFCommand(_commandConfig.Increment, 0, 3), new BFCommand("beginCaendC", BFCommandType.Trivia, 0, 6) } },
            new object[] { "beginCaendCincbeginCaendCinc", new[] { new BFCommand("beginCaendC", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 11), new BFCommand("beginCaendC", BFCommandType.Trivia, 0, 14), new BFCommand(_commandConfig.Increment, 0, 25) } },
            new object[] { "incbeginCaendCincbeginCaendC", new[] { _commandConfig.Increment, new BFCommand("beginCaendC", BFCommandType.Trivia, 0, 3), new BFCommand(_commandConfig.Increment, 0, 14), new BFCommand("beginCaendC", BFCommandType.Trivia, 0, 17) } },
            new object[] { "beginCaendCincincbeginCaendC", new[] { new BFCommand("beginCaendC", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 11), new BFCommand(_commandConfig.Increment, 0, 14), new BFCommand("beginCaendC", BFCommandType.Trivia, 0, 17) } },
            new object[] { "beginCaendCincbeginCaendCincbeginCaendC", new[] { new BFCommand("beginCaendC", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 11), new BFCommand("beginCaendC", BFCommandType.Trivia, 0, 14), new BFCommand(_commandConfig.Increment, 0, 25), new BFCommand("beginCaendC", BFCommandType.Trivia, 0, 28) } },
            new object[] { "beginCaendCinc inc ", new[] { new BFCommand("beginCaendC", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 11), new BFCommand(" ", BFCommandType.Trivia, 0, 14), new BFCommand(_commandConfig.Increment, 0, 15), new BFCommand(" ", BFCommandType.Trivia, 0, 18) } },
            new object[] { " incbeginCaendCinc ", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 1), new BFCommand("beginCaendC", BFCommandType.Trivia, 0, 4), new BFCommand(_commandConfig.Increment, 0, 15), new BFCommand(" ", BFCommandType.Trivia, 0, 18), } },
            new object[] { " inc incbeginCaendC", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 1), new BFCommand(" ", BFCommandType.Trivia, 0, 4), new BFCommand(_commandConfig.Increment, 0, 5), new BFCommand("beginCaendC", BFCommandType.Trivia, 0, 8) } },
            new object[] { "beginCaendCincbeginCaendCinc ", new[] { new BFCommand("beginCaendC", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 11), new BFCommand("beginCaendC", BFCommandType.Trivia, 0, 14), new BFCommand(_commandConfig.Increment, 0, 25), new BFCommand(" ", BFCommandType.Trivia, 0, 28) } },
            new object[] { "beginCaendCinc incbeginCaendC", new[] { new BFCommand("beginCaendC", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 11), new BFCommand(" ", BFCommandType.Trivia, 0, 14), new BFCommand(_commandConfig.Increment, 0, 15), new BFCommand("beginCaendC", BFCommandType.Trivia, 0, 18) } },
            new object[] { " incbeginCaendCincbeginCaendC", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 1), new BFCommand("beginCaendC", BFCommandType.Trivia, 0, 4), new BFCommand(_commandConfig.Increment, 0, 15), new BFCommand("beginCaendC", BFCommandType.Trivia, 0, 18) } },
            new object[] { " inc incbeginCaendC", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 1), new BFCommand(" ", BFCommandType.Trivia, 0, 4), new BFCommand(_commandConfig.Increment, 0, 5), new BFCommand("beginCaendC", BFCommandType.Trivia, 0, 8) } },
            new object[] { " incbeginCaendCinc ", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 1), new BFCommand("beginCaendC", BFCommandType.Trivia, 0, 4), new BFCommand(_commandConfig.Increment, 0, 15), new BFCommand(" ", BFCommandType.Trivia, 0, 18) } },
            new object[] { "beginCaendCinc inc ", new[] { new BFCommand("beginCaendC", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 11), new BFCommand(" ", BFCommandType.Trivia, 0, 14), new BFCommand(_commandConfig.Increment, 0, 15), new BFCommand(" ", BFCommandType.Trivia, 0, 18) } },
            new object[] { " beginCaendCincbeginCaendCincbeginCaendC", new[] { new BFCommand(" beginCaendC", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 12), new BFCommand("beginCaendC", BFCommandType.Trivia, 0, 15), new BFCommand(_commandConfig.Increment, 0, 26), new BFCommand("beginCaendC", BFCommandType.Trivia, 0, 29) } },
            new object[] { "beginCaendC incbeginCaendCincbeginCaendC", new[] { new BFCommand("beginCaendC ", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 12), new BFCommand("beginCaendC", BFCommandType.Trivia, 0, 15), new BFCommand(_commandConfig.Increment, 0, 26), new BFCommand("beginCaendC", BFCommandType.Trivia, 0, 29) } },
            new object[] { "beginCaendCinc beginCaendCincbeginCaendC", new[] { new BFCommand("beginCaendC", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 11), new BFCommand(" beginCaendC", BFCommandType.Trivia, 0, 14), new BFCommand(_commandConfig.Increment, 0, 26), new BFCommand("beginCaendC", BFCommandType.Trivia, 0, 29) } },
            new object[] { "beginCaendCincbeginCaendC incbeginCaendC", new[] { new BFCommand("beginCaendC", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 11), new BFCommand("beginCaendC ", BFCommandType.Trivia, 0, 14), new BFCommand(_commandConfig.Increment, 0, 26), new BFCommand("beginCaendC", BFCommandType.Trivia, 0, 29) } },
            new object[] { "beginCaendCincbeginCaendCinc beginCaendC", new[] { new BFCommand("beginCaendC", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 11), new BFCommand("beginCaendC", BFCommandType.Trivia, 0, 14), new BFCommand(_commandConfig.Increment, 0, 25), new BFCommand(" beginCaendC", BFCommandType.Trivia, 0, 28) } },
            new object[] { "beginCaendCincbeginCaendCincbeginCaendC ", new[] { new BFCommand("beginCaendC", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 11), new BFCommand("beginCaendC", BFCommandType.Trivia, 0, 14), new BFCommand(_commandConfig.Increment, 0, 25), new BFCommand("beginCaendC ", BFCommandType.Trivia, 0, 28) } },
            new object[] { " beginCaendC incbeginCaendCincbeginCaendC", new[] { new BFCommand(" beginCaendC ", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 13), new BFCommand("beginCaendC", BFCommandType.Trivia, 0, 16), new BFCommand(_commandConfig.Increment, 0, 27), new BFCommand("beginCaendC", BFCommandType.Trivia, 0, 30) } },
            new object[] { " beginCaendCinc beginCaendCincbeginCaendC", new[] { new BFCommand(" beginCaendC", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 12), new BFCommand(" beginCaendC", BFCommandType.Trivia, 0, 15), new BFCommand(_commandConfig.Increment, 0, 27), new BFCommand("beginCaendC", BFCommandType.Trivia, 0, 30) } },
            new object[] { " beginCaendCincbeginCaendC incbeginCaendC", new[] { new BFCommand(" beginCaendC", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 12), new BFCommand("beginCaendC ", BFCommandType.Trivia, 0, 15), new BFCommand(_commandConfig.Increment, 0, 27), new BFCommand("beginCaendC", BFCommandType.Trivia, 0, 30) } },
            new object[] { " beginCaendCincbeginCaendCinc beginCaendC", new[] { new BFCommand(" beginCaendC", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 12), new BFCommand("beginCaendC", BFCommandType.Trivia, 0, 15), new BFCommand(_commandConfig.Increment, 0, 26), new BFCommand(" beginCaendC", BFCommandType.Trivia, 0, 29) } },
            new object[] { " beginCaendCincbeginCaendCincbeginCaendC ", new[] { new BFCommand(" beginCaendC", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 12), new BFCommand("beginCaendC", BFCommandType.Trivia, 0, 15), new BFCommand(_commandConfig.Increment, 0, 26), new BFCommand("beginCaendC ", BFCommandType.Trivia, 0, 29) } },
            new object[] { " beginCaendC inc beginCaendCincbeginCaendC", new[] { new BFCommand(" beginCaendC ", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 13), new BFCommand(" beginCaendC", BFCommandType.Trivia, 0, 16), new BFCommand(_commandConfig.Increment, 0, 28), new BFCommand("beginCaendC", BFCommandType.Trivia, 0, 31) } },
            new object[] { " beginCaendC incbeginCaendC incbeginCaendC", new[] { new BFCommand(" beginCaendC ", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 13), new BFCommand("beginCaendC ", BFCommandType.Trivia, 0, 16), new BFCommand(_commandConfig.Increment, 0, 28), new BFCommand("beginCaendC", BFCommandType.Trivia, 0, 31) } },
            new object[] { " beginCaendC incbeginCaendCinc beginCaendC", new[] { new BFCommand(" beginCaendC ", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 13), new BFCommand("beginCaendC", BFCommandType.Trivia, 0, 16), new BFCommand(_commandConfig.Increment, 0, 27), new BFCommand(" beginCaendC", BFCommandType.Trivia, 0, 30) } },
            new object[] { " beginCaendC incbeginCaendCincbeginCaendC ", new[] { new BFCommand(" beginCaendC ", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 13), new BFCommand("beginCaendC", BFCommandType.Trivia, 0, 16), new BFCommand(_commandConfig.Increment, 0, 27), new BFCommand("beginCaendC ", BFCommandType.Trivia, 0, 30) } },
            new object[] { " beginCaendCinc beginCaendC incbeginCaendC", new[] { new BFCommand(" beginCaendC", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 12), new BFCommand(" beginCaendC ", BFCommandType.Trivia, 0, 15), new BFCommand(_commandConfig.Increment, 0, 28), new BFCommand("beginCaendC", BFCommandType.Trivia, 0, 31) } },
            new object[] { " beginCaendCinc beginCaendCinc beginCaendC", new[] { new BFCommand(" beginCaendC", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 12), new BFCommand(" beginCaendC", BFCommandType.Trivia, 0, 15), new BFCommand(_commandConfig.Increment, 0, 27), new BFCommand(" beginCaendC", BFCommandType.Trivia, 0, 30) } },
            new object[] { " beginCaendCinc beginCaendCincbeginCaendC ", new[] { new BFCommand(" beginCaendC", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 12), new BFCommand(" beginCaendC", BFCommandType.Trivia, 0, 15), new BFCommand(_commandConfig.Increment, 0, 27), new BFCommand("beginCaendC ", BFCommandType.Trivia, 0, 30) } },
            new object[] { " beginCaendCincbeginCaendC inc beginCaendC", new[] { new BFCommand(" beginCaendC", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 12), new BFCommand("beginCaendC ", BFCommandType.Trivia, 0, 15), new BFCommand(_commandConfig.Increment, 0, 27), new BFCommand(" beginCaendC", BFCommandType.Trivia, 0, 30) } },
            new object[] { " beginCaendCincbeginCaendC incbeginCaendC ", new[] { new BFCommand(" beginCaendC", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 12), new BFCommand("beginCaendC ", BFCommandType.Trivia, 0, 15), new BFCommand(_commandConfig.Increment, 0, 27), new BFCommand("beginCaendC ", BFCommandType.Trivia, 0, 30) } },
            new object[] { " beginCaendCincbeginCaendCinc beginCaendC ", new[] { new BFCommand(" beginCaendC", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 12), new BFCommand("beginCaendC", BFCommandType.Trivia, 0, 15), new BFCommand(_commandConfig.Increment, 0, 26), new BFCommand(" beginCaendC ", BFCommandType.Trivia, 0, 29) } },
            new object[] { " beginCaendC inc beginCaendC incbeginCaendC", new[] { new BFCommand(" beginCaendC ", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 13), new BFCommand(" beginCaendC ", BFCommandType.Trivia, 0, 16), new BFCommand(_commandConfig.Increment, 0, 29), new BFCommand("beginCaendC", BFCommandType.Trivia, 0, 32) } },
            new object[] { " beginCaendC inc beginCaendCinc beginCaendC", new[] { new BFCommand(" beginCaendC ", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 13), new BFCommand(" beginCaendC", BFCommandType.Trivia, 0, 16), new BFCommand(_commandConfig.Increment, 0, 28), new BFCommand(" beginCaendC", BFCommandType.Trivia, 0, 31) } },
            new object[] { " beginCaendC inc beginCaendCincbeginCaendC ", new[] { new BFCommand(" beginCaendC ", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 13), new BFCommand(" beginCaendC", BFCommandType.Trivia, 0, 16), new BFCommand(_commandConfig.Increment, 0, 28), new BFCommand("beginCaendC ", BFCommandType.Trivia, 0, 31) } },
            new object[] { " beginCaendC inc beginCaendC inc beginCaendC", new[] { new BFCommand(" beginCaendC ", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 13), new BFCommand(" beginCaendC ", BFCommandType.Trivia, 0, 16), new BFCommand(_commandConfig.Increment, 0, 29), new BFCommand(" beginCaendC", BFCommandType.Trivia, 0, 32) } },
            new object[] { " beginCaendC inc beginCaendC incbeginCaendC ", new[] { new BFCommand(" beginCaendC ", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 13), new BFCommand(" beginCaendC ", BFCommandType.Trivia, 0, 16), new BFCommand(_commandConfig.Increment, 0, 29), new BFCommand("beginCaendC ", BFCommandType.Trivia, 0, 32) } },
            new object[] { " beginCaendC inc beginCaendC inc beginCaendC ", new[] { new BFCommand(" beginCaendC ", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 13), new BFCommand(" beginCaendC ", BFCommandType.Trivia, 0, 16), new BFCommand(_commandConfig.Increment, 0, 29), new BFCommand(" beginCaendC ", BFCommandType.Trivia, 0, 32) } },
        };

        [TestMethod]
        [TestCaseSource(nameof(ParseCorrectlyWithCommentTestSource))]
        public void ParseCorrectlyWithCommentTest()
        {
            this.TestContext.Run<string, IEnumerable<BFCommand>>((code, expected) =>
            {
                var parsed = this._parser.Parse(code).ToArray();
                parsed.Is(expected, $@"TestCase: ""{code}""");
            });
        }
    }
}
