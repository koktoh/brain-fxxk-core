﻿using System.Collections.Generic;
using System.Linq;
using BFCore.Command;
using BFCore.Config;
using BFCore.Extesion;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace brain_fxxk_core.test.Parse.Default
{
    [TestClass]
    public class ParseCustomCommandCorrectlyTest : DefaultParserTestBase
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
        [TestCase("beginCa")]
        [TestCase("beginCaendC")]
        [TestCase("beginCaincendC")]
        [TestCase("beginCainc")]
        [TestCase(" beginCa")]
        [TestCase(" beginCaendC")]
        [TestCase(" beginCaincendC")]
        [TestCase(" beginCainc ")]
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
            new object[] { "beginC", new[] { _commandConfig.BeginComment } },
            new object[] { "endC", new[] { _commandConfig.EndComment } },
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
            new object[] { "beginCaendCincinc", new[] { _commandConfig.BeginComment, new BFCommand("a", BFCommandType.Trivia, 0, 6), new BFCommand(_commandConfig.EndComment, 0, 7), new BFCommand(_commandConfig.Increment, 0, 11), new BFCommand(_commandConfig.Increment, 0, 14) } },
            new object[] { "incbeginCaendCinc", new[] { _commandConfig.Increment, new BFCommand(_commandConfig.BeginComment, 0, 3), new BFCommand("a", BFCommandType.Trivia, 0, 9), new BFCommand(_commandConfig.EndComment, 0, 10), new BFCommand(_commandConfig.Increment, 0, 14) } },
            new object[] { "incincbeginCaendC", new[] { _commandConfig.Increment, new BFCommand(_commandConfig.Increment, 0, 3), new BFCommand(_commandConfig.BeginComment, 0, 6), new BFCommand("a", BFCommandType.Trivia, 0, 12), new BFCommand(_commandConfig.EndComment, 0, 13) } },
            new object[] { "beginCaendCincbeginCaendCinc", new[] { _commandConfig.BeginComment, new BFCommand("a", BFCommandType.Trivia, 0, 6), new BFCommand(_commandConfig.EndComment, 0, 7), new BFCommand(_commandConfig.Increment, 0, 11), new BFCommand(_commandConfig.BeginComment, 0, 14), new BFCommand("a", BFCommandType.Trivia, 0, 20), new BFCommand(_commandConfig.EndComment, 0, 21), new BFCommand(_commandConfig.Increment, 0, 25) } },
            new object[] { "incbeginCaendCincbeginCaendC", new[] { _commandConfig.Increment, new BFCommand(_commandConfig.BeginComment, 0, 3), new BFCommand("a", BFCommandType.Trivia, 0, 9), new BFCommand(_commandConfig.EndComment, 0, 10), new BFCommand(_commandConfig.Increment, 0, 14), new BFCommand(_commandConfig.BeginComment, 0, 17), new BFCommand("a", BFCommandType.Trivia, 0, 23), new BFCommand(_commandConfig.EndComment, 0, 24) } },
            new object[] { "beginCaendCincincbeginCaendC", new[] { _commandConfig.BeginComment, new BFCommand("a", BFCommandType.Trivia, 0, 6), new BFCommand(_commandConfig.EndComment, 0, 7), new BFCommand(_commandConfig.Increment, 0, 11), new BFCommand(_commandConfig.Increment, 0, 14), new BFCommand(_commandConfig.BeginComment, 0, 17), new BFCommand("a", BFCommandType.Trivia, 0, 23), new BFCommand(_commandConfig.EndComment, 0, 24) } },
            new object[] { "beginCaendCincbeginCaendCincbeginCaendC", new[] { _commandConfig.BeginComment, new BFCommand("a", BFCommandType.Trivia, 0, 6), new BFCommand(_commandConfig.EndComment, 0, 7), new BFCommand(_commandConfig.Increment, 0, 11), new BFCommand(_commandConfig.BeginComment, 0, 14), new BFCommand("a", BFCommandType.Trivia, 0, 20), new BFCommand(_commandConfig.EndComment, 0, 21), new BFCommand(_commandConfig.Increment, 0, 25), new BFCommand(_commandConfig.BeginComment, 0, 28), new BFCommand("a", BFCommandType.Trivia, 0, 34), new BFCommand(_commandConfig.EndComment, 0, 35) } },
            new object[] { "beginCaendCinc inc ", new[] { _commandConfig.BeginComment, new BFCommand("a", BFCommandType.Trivia, 0, 6), new BFCommand(_commandConfig.EndComment, 0, 7), new BFCommand(_commandConfig.Increment, 0, 11), new BFCommand(" ", BFCommandType.Trivia, 0, 14), new BFCommand(_commandConfig.Increment, 0, 15), new BFCommand(" ", BFCommandType.Trivia, 0, 18) } },
            new object[] { " incbeginCaendCinc ", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 1), new BFCommand(_commandConfig.BeginComment, 0, 4), new BFCommand("a", BFCommandType.Trivia, 0, 10), new BFCommand(_commandConfig.EndComment, 0, 11), new BFCommand(_commandConfig.Increment, 0, 15), new BFCommand(" ", BFCommandType.Trivia, 0, 18), } },
            new object[] { " inc incbeginCaendC", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 1), new BFCommand(" ", BFCommandType.Trivia, 0, 4), new BFCommand(_commandConfig.Increment, 0, 5), new BFCommand(_commandConfig.BeginComment, 0, 8), new BFCommand("a", BFCommandType.Trivia, 0, 14), new BFCommand(_commandConfig.EndComment, 0, 15) } },
            new object[] { "beginCaendCincbeginCaendCinc ", new[] { _commandConfig.BeginComment, new BFCommand("a", BFCommandType.Trivia, 0, 6), new BFCommand(_commandConfig.EndComment, 0, 7), new BFCommand(_commandConfig.Increment, 0, 11), new BFCommand(_commandConfig.BeginComment, 0, 14), new BFCommand("a", BFCommandType.Trivia, 0, 20), new BFCommand(_commandConfig.EndComment, 0, 21), new BFCommand(_commandConfig.Increment, 0, 25), new BFCommand(" ", BFCommandType.Trivia, 0, 28) } },
            new object[] { "beginCaendCinc incbeginCaendC", new[] { _commandConfig.BeginComment, new BFCommand("a", BFCommandType.Trivia, 0, 6), new BFCommand(_commandConfig.EndComment, 0, 7), new BFCommand(_commandConfig.Increment, 0, 11), new BFCommand(" ", BFCommandType.Trivia, 0, 14), new BFCommand(_commandConfig.Increment, 0, 15), new BFCommand(_commandConfig.BeginComment, 0, 18), new BFCommand("a", BFCommandType.Trivia, 0, 24), new BFCommand(_commandConfig.EndComment, 0, 25) } },
            new object[] { " incbeginCaendCincbeginCaendC", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 1), new BFCommand(_commandConfig.BeginComment, 0, 4), new BFCommand("a", BFCommandType.Trivia, 0, 10), new BFCommand(_commandConfig.EndComment, 0, 11), new BFCommand(_commandConfig.Increment, 0, 15), new BFCommand(_commandConfig.BeginComment, 0, 18), new BFCommand("a", BFCommandType.Trivia, 0, 24), new BFCommand(_commandConfig.EndComment, 0, 25) } },
            new object[] { " inc incbeginCaendC", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 1), new BFCommand(" ", BFCommandType.Trivia, 0, 4), new BFCommand(_commandConfig.Increment, 0, 5), new BFCommand(_commandConfig.BeginComment, 0, 8), new BFCommand("a", BFCommandType.Trivia, 0, 14), new BFCommand(_commandConfig.EndComment, 0, 15) } },
            new object[] { " incbeginCaendCinc ", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_commandConfig.Increment, 0, 1), new BFCommand(_commandConfig.BeginComment, 0, 4), new BFCommand("a", BFCommandType.Trivia, 0, 10), new BFCommand(_commandConfig.EndComment, 0, 11), new BFCommand(_commandConfig.Increment, 0, 15), new BFCommand(" ", BFCommandType.Trivia, 0, 18) } },
            new object[] { "beginCaendCinc inc ", new[] { _commandConfig.BeginComment, new BFCommand("a", BFCommandType.Trivia, 0, 6), new BFCommand(_commandConfig.EndComment, 0, 7), new BFCommand(_commandConfig.Increment, 0, 11), new BFCommand(" ", BFCommandType.Trivia, 0, 14), new BFCommand(_commandConfig.Increment, 0, 15), new BFCommand(" ", BFCommandType.Trivia, 0, 18) } },
            new object[] { " beginCaendCincbeginCaendCincbeginCaendC", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_commandConfig.BeginComment, 0, 1), new BFCommand("a", BFCommandType.Trivia, 0, 7), new BFCommand(_commandConfig.EndComment, 0, 8), new BFCommand(_commandConfig.Increment, 0, 12), new BFCommand(_commandConfig.BeginComment, 0, 15), new BFCommand("a", BFCommandType.Trivia, 0, 21), new BFCommand(_commandConfig.EndComment, 0, 22), new BFCommand(_commandConfig.Increment, 0, 26), new BFCommand(_commandConfig.BeginComment, 0, 29), new BFCommand("a", BFCommandType.Trivia, 0, 35), new BFCommand(_commandConfig.EndComment, 0, 36) } },
            new object[] { "beginCaendC incbeginCaendCincbeginCaendC", new[] { _commandConfig.BeginComment, new BFCommand("a", BFCommandType.Trivia, 0, 6), new BFCommand(_commandConfig.EndComment, 0, 7), new BFCommand(" ", BFCommandType.Trivia, 0, 11), new BFCommand(_commandConfig.Increment, 0, 12), new BFCommand(_commandConfig.BeginComment, 0, 15), new BFCommand("a", BFCommandType.Trivia, 0, 21), new BFCommand(_commandConfig.EndComment, 0, 22), new BFCommand(_commandConfig.Increment, 0, 26), new BFCommand(_commandConfig.BeginComment, 0, 29), new BFCommand("a", BFCommandType.Trivia, 0, 35), new BFCommand(_commandConfig.EndComment, 0, 36) } },
            new object[] { "beginCaendCinc beginCaendCincbeginCaendC", new[] { _commandConfig.BeginComment, new BFCommand("a", BFCommandType.Trivia, 0, 6), new BFCommand(_commandConfig.EndComment, 0, 7), new BFCommand(_commandConfig.Increment, 0, 11), new BFCommand(" ", BFCommandType.Trivia, 0, 14), new BFCommand(_commandConfig.BeginComment, 0, 15), new BFCommand("a", BFCommandType.Trivia, 0, 21), new BFCommand(_commandConfig.EndComment, 0, 22), new BFCommand(_commandConfig.Increment, 0, 26), new BFCommand(_commandConfig.BeginComment, 0, 29), new BFCommand("a", BFCommandType.Trivia, 0, 35), new BFCommand(_commandConfig.EndComment, 0, 36) } },
            new object[] { "beginCaendCincbeginCaendC incbeginCaendC", new[] { _commandConfig.BeginComment, new BFCommand("a", BFCommandType.Trivia, 0, 6), new BFCommand(_commandConfig.EndComment, 0, 7), new BFCommand(_commandConfig.Increment, 0, 11), new BFCommand(_commandConfig.BeginComment, 0, 14), new BFCommand("a", BFCommandType.Trivia, 0, 20), new BFCommand(_commandConfig.EndComment, 0, 21), new BFCommand(" ", BFCommandType.Trivia, 0, 25), new BFCommand(_commandConfig.Increment, 0, 26), new BFCommand(_commandConfig.BeginComment, 0, 29), new BFCommand("a", BFCommandType.Trivia, 0, 35), new BFCommand(_commandConfig.EndComment, 0, 36) } },
            new object[] { "beginCaendCincbeginCaendCinc beginCaendC", new[] { _commandConfig.BeginComment, new BFCommand("a", BFCommandType.Trivia, 0, 6), new BFCommand(_commandConfig.EndComment, 0, 7), new BFCommand(_commandConfig.Increment, 0, 11), new BFCommand(_commandConfig.BeginComment, 0, 14), new BFCommand("a", BFCommandType.Trivia, 0, 20), new BFCommand(_commandConfig.EndComment, 0, 21), new BFCommand(_commandConfig.Increment, 0, 25), new BFCommand(" ", BFCommandType.Trivia, 0, 28), new BFCommand(_commandConfig.BeginComment, 0, 29), new BFCommand("a", BFCommandType.Trivia, 0, 35), new BFCommand(_commandConfig.EndComment, 0, 36) } },
            new object[] { "beginCaendCincbeginCaendCincbeginCaendC ", new[] { _commandConfig.BeginComment, new BFCommand("a", BFCommandType.Trivia, 0, 6), new BFCommand(_commandConfig.EndComment, 0, 7), new BFCommand(_commandConfig.Increment, 0, 11), new BFCommand(_commandConfig.BeginComment, 0, 14), new BFCommand("a", BFCommandType.Trivia, 0, 20), new BFCommand(_commandConfig.EndComment, 0, 21), new BFCommand(_commandConfig.Increment, 0, 25), new BFCommand(_commandConfig.BeginComment, 0, 28), new BFCommand("a", BFCommandType.Trivia, 0, 34), new BFCommand(_commandConfig.EndComment, 0, 35), new BFCommand(" ", BFCommandType.Trivia, 0, 39) } },
            new object[] { " beginCaendC incbeginCaendCincbeginCaendC", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_commandConfig.BeginComment, 0, 1), new BFCommand("a", BFCommandType.Trivia, 0, 7), new BFCommand(_commandConfig.EndComment, 0, 8), new BFCommand(" ", BFCommandType.Trivia, 0, 12), new BFCommand(_commandConfig.Increment, 0, 13), new BFCommand(_commandConfig.BeginComment, 0, 16), new BFCommand("a", BFCommandType.Trivia, 0, 22), new BFCommand(_commandConfig.EndComment, 0, 23), new BFCommand(_commandConfig.Increment, 0, 27), new BFCommand(_commandConfig.BeginComment, 0, 30), new BFCommand("a", BFCommandType.Trivia, 0, 36), new BFCommand(_commandConfig.EndComment, 0, 37) } },
            new object[] { " beginCaendCinc beginCaendCincbeginCaendC", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_commandConfig.BeginComment, 0, 1), new BFCommand("a", BFCommandType.Trivia, 0, 7), new BFCommand(_commandConfig.EndComment, 0, 8), new BFCommand(_commandConfig.Increment, 0, 12), new BFCommand(" ", BFCommandType.Trivia, 0, 15), new BFCommand(_commandConfig.BeginComment, 0, 16), new BFCommand("a", BFCommandType.Trivia, 0, 22), new BFCommand(_commandConfig.EndComment, 0, 23), new BFCommand(_commandConfig.Increment, 0, 27), new BFCommand(_commandConfig.BeginComment, 0, 30), new BFCommand("a", BFCommandType.Trivia, 0, 36), new BFCommand(_commandConfig.EndComment, 0, 37) } },
            new object[] { " beginCaendCincbeginCaendC incbeginCaendC", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_commandConfig.BeginComment, 0, 1), new BFCommand("a", BFCommandType.Trivia, 0, 7), new BFCommand(_commandConfig.EndComment, 0, 8), new BFCommand(_commandConfig.Increment, 0, 12), new BFCommand(_commandConfig.BeginComment, 0, 15), new BFCommand("a", BFCommandType.Trivia, 0, 21), new BFCommand(_commandConfig.EndComment, 0, 22), new BFCommand(" ", BFCommandType.Trivia, 0, 26), new BFCommand(_commandConfig.Increment, 0, 27), new BFCommand(_commandConfig.BeginComment, 0, 30), new BFCommand("a", BFCommandType.Trivia, 0, 36), new BFCommand(_commandConfig.EndComment, 0, 37) } },
            new object[] { " beginCaendCincbeginCaendCinc beginCaendC", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_commandConfig.BeginComment, 0, 1), new BFCommand("a", BFCommandType.Trivia, 0, 7), new BFCommand(_commandConfig.EndComment, 0, 8), new BFCommand(_commandConfig.Increment, 0, 12), new BFCommand(_commandConfig.BeginComment, 0, 15), new BFCommand("a", BFCommandType.Trivia, 0, 21), new BFCommand(_commandConfig.EndComment, 0, 22), new BFCommand(_commandConfig.Increment, 0, 26), new BFCommand(" ", BFCommandType.Trivia, 0, 29), new BFCommand(_commandConfig.BeginComment, 0, 30), new BFCommand("a", BFCommandType.Trivia, 0, 36), new BFCommand(_commandConfig.EndComment, 0, 37) } },
            new object[] { " beginCaendCincbeginCaendCincbeginCaendC ", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_commandConfig.BeginComment, 0, 1), new BFCommand("a", BFCommandType.Trivia, 0, 7), new BFCommand(_commandConfig.EndComment, 0, 8), new BFCommand(_commandConfig.Increment, 0, 12), new BFCommand(_commandConfig.BeginComment, 0, 15), new BFCommand("a", BFCommandType.Trivia, 0, 21), new BFCommand(_commandConfig.EndComment, 0, 22), new BFCommand(_commandConfig.Increment, 0, 26), new BFCommand(_commandConfig.BeginComment, 0, 29), new BFCommand("a", BFCommandType.Trivia, 0, 35), new BFCommand(_commandConfig.EndComment, 0, 36), new BFCommand(" ", BFCommandType.Trivia, 0, 40) } },
            new object[] { " beginCaendC inc beginCaendCincbeginCaendC", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_commandConfig.BeginComment, 0, 1), new BFCommand("a", BFCommandType.Trivia, 0, 7), new BFCommand(_commandConfig.EndComment, 0, 8), new BFCommand(" ", BFCommandType.Trivia, 0, 12), new BFCommand(_commandConfig.Increment, 0, 13), new BFCommand(" ", BFCommandType.Trivia, 0, 16), new BFCommand(_commandConfig.BeginComment, 0, 17), new BFCommand("a", BFCommandType.Trivia, 0, 23), new BFCommand(_commandConfig.EndComment, 0, 24), new BFCommand(_commandConfig.Increment, 0, 28), new BFCommand(_commandConfig.BeginComment, 0, 31), new BFCommand("a", BFCommandType.Trivia, 0, 37), new BFCommand(_commandConfig.EndComment, 0, 38) } },
            new object[] { " beginCaendC incbeginCaendC incbeginCaendC", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_commandConfig.BeginComment, 0, 1), new BFCommand("a", BFCommandType.Trivia, 0, 7), new BFCommand(_commandConfig.EndComment, 0, 8), new BFCommand(" ", BFCommandType.Trivia, 0, 12), new BFCommand(_commandConfig.Increment, 0, 13), new BFCommand(_commandConfig.BeginComment, 0, 16), new BFCommand("a", BFCommandType.Trivia, 0, 22), new BFCommand(_commandConfig.EndComment, 0, 23), new BFCommand(" ", BFCommandType.Trivia, 0, 27), new BFCommand(_commandConfig.Increment, 0, 28), new BFCommand(_commandConfig.BeginComment, 0, 31), new BFCommand("a", BFCommandType.Trivia, 0, 37), new BFCommand(_commandConfig.EndComment, 0, 38) } },
            new object[] { " beginCaendC incbeginCaendCinc beginCaendC", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_commandConfig.BeginComment, 0, 1), new BFCommand("a", BFCommandType.Trivia, 0, 7), new BFCommand(_commandConfig.EndComment, 0, 8), new BFCommand(" ", BFCommandType.Trivia, 0, 12), new BFCommand(_commandConfig.Increment, 0, 13), new BFCommand(_commandConfig.BeginComment, 0, 16), new BFCommand("a", BFCommandType.Trivia, 0, 22), new BFCommand(_commandConfig.EndComment, 0, 23), new BFCommand(_commandConfig.Increment, 0, 27), new BFCommand(" ", BFCommandType.Trivia, 0, 30), new BFCommand(_commandConfig.BeginComment, 0, 31), new BFCommand("a", BFCommandType.Trivia, 0, 37), new BFCommand(_commandConfig.EndComment, 0, 38) } },
            new object[] { " beginCaendC incbeginCaendCincbeginCaendC ", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_commandConfig.BeginComment, 0, 1), new BFCommand("a", BFCommandType.Trivia, 0, 7), new BFCommand(_commandConfig.EndComment, 0, 8), new BFCommand(" ", BFCommandType.Trivia, 0, 12), new BFCommand(_commandConfig.Increment, 0, 13), new BFCommand(_commandConfig.BeginComment, 0, 16), new BFCommand("a", BFCommandType.Trivia, 0, 22), new BFCommand(_commandConfig.EndComment, 0, 23), new BFCommand(_commandConfig.Increment, 0, 27), new BFCommand(_commandConfig.BeginComment, 0, 30), new BFCommand("a", BFCommandType.Trivia, 0, 36), new BFCommand(_commandConfig.EndComment, 0, 37), new BFCommand(" ", BFCommandType.Trivia, 0, 41) } },
            new object[] { " beginCaendCinc beginCaendC incbeginCaendC", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_commandConfig.BeginComment, 0, 1), new BFCommand("a", BFCommandType.Trivia, 0, 7), new BFCommand(_commandConfig.EndComment, 0, 8), new BFCommand(_commandConfig.Increment, 0, 12), new BFCommand(" ", BFCommandType.Trivia, 0, 15), new BFCommand(_commandConfig.BeginComment, 0, 16), new BFCommand("a", BFCommandType.Trivia, 0, 22), new BFCommand(_commandConfig.EndComment, 0, 23), new BFCommand(" ", BFCommandType.Trivia, 0, 27), new BFCommand(_commandConfig.Increment, 0, 28), new BFCommand(_commandConfig.BeginComment, 0, 31), new BFCommand("a", BFCommandType.Trivia, 0, 37), new BFCommand(_commandConfig.EndComment, 0, 38) } },
            new object[] { " beginCaendCinc beginCaendCinc beginCaendC", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_commandConfig.BeginComment, 0, 1), new BFCommand("a", BFCommandType.Trivia, 0, 7), new BFCommand(_commandConfig.EndComment, 0, 8), new BFCommand(_commandConfig.Increment, 0, 12), new BFCommand(" ", BFCommandType.Trivia, 0, 15), new BFCommand(_commandConfig.BeginComment, 0, 16), new BFCommand("a", BFCommandType.Trivia, 0, 22), new BFCommand(_commandConfig.EndComment, 0, 23), new BFCommand(_commandConfig.Increment, 0, 27), new BFCommand(" ", BFCommandType.Trivia, 0, 30), new BFCommand(_commandConfig.BeginComment, 0, 31), new BFCommand("a", BFCommandType.Trivia, 0, 37), new BFCommand(_commandConfig.EndComment, 0, 38) } },
            new object[] { " beginCaendCinc beginCaendCincbeginCaendC ", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_commandConfig.BeginComment, 0, 1), new BFCommand("a", BFCommandType.Trivia, 0, 7), new BFCommand(_commandConfig.EndComment, 0, 8), new BFCommand(_commandConfig.Increment, 0, 12), new BFCommand(" ", BFCommandType.Trivia, 0, 15), new BFCommand(_commandConfig.BeginComment, 0, 16), new BFCommand("a", BFCommandType.Trivia, 0, 22), new BFCommand(_commandConfig.EndComment, 0, 23), new BFCommand(_commandConfig.Increment, 0, 27), new BFCommand(_commandConfig.BeginComment, 0, 30), new BFCommand("a", BFCommandType.Trivia, 0, 36), new BFCommand(_commandConfig.EndComment, 0, 37), new BFCommand(" ", BFCommandType.Trivia, 0, 41) } },
            new object[] { " beginCaendCincbeginCaendC inc beginCaendC", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_commandConfig.BeginComment, 0, 1), new BFCommand("a", BFCommandType.Trivia, 0, 7), new BFCommand(_commandConfig.EndComment, 0, 8), new BFCommand(_commandConfig.Increment, 0, 12), new BFCommand(_commandConfig.BeginComment, 0, 15), new BFCommand("a", BFCommandType.Trivia, 0, 21), new BFCommand(_commandConfig.EndComment, 0, 22), new BFCommand(" ", BFCommandType.Trivia, 0, 26), new BFCommand(_commandConfig.Increment, 0, 27), new BFCommand(" ", BFCommandType.Trivia, 0, 30), new BFCommand(_commandConfig.BeginComment, 0, 31), new BFCommand("a", BFCommandType.Trivia, 0, 37), new BFCommand(_commandConfig.EndComment, 0, 38) } },
            new object[] { " beginCaendCincbeginCaendC incbeginCaendC ", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_commandConfig.BeginComment, 0, 1), new BFCommand("a", BFCommandType.Trivia, 0, 7), new BFCommand(_commandConfig.EndComment, 0, 8), new BFCommand(_commandConfig.Increment, 0, 12), new BFCommand(_commandConfig.BeginComment, 0, 15), new BFCommand("a", BFCommandType.Trivia, 0, 21), new BFCommand(_commandConfig.EndComment, 0, 22), new BFCommand(" ", BFCommandType.Trivia, 0, 26), new BFCommand(_commandConfig.Increment, 0, 27), new BFCommand(_commandConfig.BeginComment, 0, 30), new BFCommand("a", BFCommandType.Trivia, 0, 36), new BFCommand(_commandConfig.EndComment, 0, 37), new BFCommand(" ", BFCommandType.Trivia, 0, 41) } },
            new object[] { " beginCaendCincbeginCaendCinc beginCaendC ", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_commandConfig.BeginComment, 0, 1), new BFCommand("a", BFCommandType.Trivia, 0, 7), new BFCommand(_commandConfig.EndComment, 0, 8), new BFCommand(_commandConfig.Increment, 0, 12), new BFCommand(_commandConfig.BeginComment, 0, 15), new BFCommand("a", BFCommandType.Trivia, 0, 21), new BFCommand(_commandConfig.EndComment, 0, 22), new BFCommand(_commandConfig.Increment, 0, 26), new BFCommand(" ", BFCommandType.Trivia, 0, 29), new BFCommand(_commandConfig.BeginComment, 0, 30), new BFCommand("a", BFCommandType.Trivia, 0, 36), new BFCommand(_commandConfig.EndComment, 0, 37), new BFCommand(" ", BFCommandType.Trivia, 0, 41) } },
            new object[] { " beginCaendC inc beginCaendC incbeginCaendC", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_commandConfig.BeginComment, 0, 1), new BFCommand("a", BFCommandType.Trivia, 0, 7), new BFCommand(_commandConfig.EndComment, 0, 8), new BFCommand(" ", BFCommandType.Trivia, 0, 12), new BFCommand(_commandConfig.Increment, 0, 13), new BFCommand(" ", BFCommandType.Trivia, 0, 16), new BFCommand(_commandConfig.BeginComment, 0, 17), new BFCommand("a", BFCommandType.Trivia, 0, 23), new BFCommand(_commandConfig.EndComment, 0, 24), new BFCommand(" ", BFCommandType.Trivia, 0, 28), new BFCommand(_commandConfig.Increment, 0, 29), new BFCommand(_commandConfig.BeginComment, 0, 32), new BFCommand("a", BFCommandType.Trivia, 0, 38), new BFCommand(_commandConfig.EndComment, 0, 39) } },
            new object[] { " beginCaendC inc beginCaendCinc beginCaendC", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_commandConfig.BeginComment, 0, 1), new BFCommand("a", BFCommandType.Trivia, 0, 7), new BFCommand(_commandConfig.EndComment, 0, 8), new BFCommand(" ", BFCommandType.Trivia, 0, 12), new BFCommand(_commandConfig.Increment, 0, 13), new BFCommand(" ", BFCommandType.Trivia, 0, 16), new BFCommand(_commandConfig.BeginComment, 0, 17), new BFCommand("a", BFCommandType.Trivia, 0, 23), new BFCommand(_commandConfig.EndComment, 0, 24), new BFCommand(_commandConfig.Increment, 0, 28), new BFCommand(" ", BFCommandType.Trivia, 0, 31), new BFCommand(_commandConfig.BeginComment, 0, 32), new BFCommand("a", BFCommandType.Trivia, 0, 38), new BFCommand(_commandConfig.EndComment, 0, 39) } },
            new object[] { " beginCaendC inc beginCaendCincbeginCaendC ", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_commandConfig.BeginComment, 0, 1), new BFCommand("a", BFCommandType.Trivia, 0, 7), new BFCommand(_commandConfig.EndComment, 0, 8), new BFCommand(" ", BFCommandType.Trivia, 0, 12), new BFCommand(_commandConfig.Increment, 0, 13), new BFCommand(" ", BFCommandType.Trivia, 0, 16), new BFCommand(_commandConfig.BeginComment, 0, 17), new BFCommand("a", BFCommandType.Trivia, 0, 23), new BFCommand(_commandConfig.EndComment, 0, 24), new BFCommand(_commandConfig.Increment, 0, 28), new BFCommand(_commandConfig.BeginComment, 0, 31), new BFCommand("a", BFCommandType.Trivia, 0, 37), new BFCommand(_commandConfig.EndComment, 0, 38), new BFCommand(" ", BFCommandType.Trivia, 0, 42) } },
            new object[] { " beginCaendC inc beginCaendC inc beginCaendC", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_commandConfig.BeginComment, 0, 1), new BFCommand("a", BFCommandType.Trivia, 0, 7), new BFCommand(_commandConfig.EndComment, 0, 8), new BFCommand(" ", BFCommandType.Trivia, 0, 12), new BFCommand(_commandConfig.Increment, 0, 13), new BFCommand(" ", BFCommandType.Trivia, 0, 16), new BFCommand(_commandConfig.BeginComment, 0, 17), new BFCommand("a", BFCommandType.Trivia, 0, 23), new BFCommand(_commandConfig.EndComment, 0, 24), new BFCommand(" ", BFCommandType.Trivia, 0, 28), new BFCommand(_commandConfig.Increment, 0, 29), new BFCommand(" ", BFCommandType.Trivia, 0, 32), new BFCommand(_commandConfig.BeginComment, 0, 33), new BFCommand("a", BFCommandType.Trivia, 0, 39), new BFCommand(_commandConfig.EndComment, 0, 40) } },
            new object[] { " beginCaendC inc beginCaendC incbeginCaendC ", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_commandConfig.BeginComment, 0, 1), new BFCommand("a", BFCommandType.Trivia, 0, 7), new BFCommand(_commandConfig.EndComment, 0, 8), new BFCommand(" ", BFCommandType.Trivia, 0, 12), new BFCommand(_commandConfig.Increment, 0, 13), new BFCommand(" ", BFCommandType.Trivia, 0, 16), new BFCommand(_commandConfig.BeginComment, 0, 17), new BFCommand("a", BFCommandType.Trivia, 0, 23), new BFCommand(_commandConfig.EndComment, 0, 24), new BFCommand(" ", BFCommandType.Trivia, 0, 28), new BFCommand(_commandConfig.Increment, 0, 29), new BFCommand(_commandConfig.BeginComment, 0, 32), new BFCommand("a", BFCommandType.Trivia, 0, 38), new BFCommand(_commandConfig.EndComment, 0, 39), new BFCommand(" ", BFCommandType.Trivia, 0, 43) } },
            new object[] { " beginCaendC inc beginCaendC inc beginCaendC ", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_commandConfig.BeginComment, 0, 1), new BFCommand("a", BFCommandType.Trivia, 0, 7), new BFCommand(_commandConfig.EndComment, 0, 8), new BFCommand(" ", BFCommandType.Trivia, 0, 12), new BFCommand(_commandConfig.Increment, 0, 13), new BFCommand(" ", BFCommandType.Trivia, 0, 16), new BFCommand(_commandConfig.BeginComment, 0, 17), new BFCommand("a", BFCommandType.Trivia, 0, 23), new BFCommand(_commandConfig.EndComment, 0, 24), new BFCommand(" ", BFCommandType.Trivia, 0, 28), new BFCommand(_commandConfig.Increment, 0, 29), new BFCommand(" ", BFCommandType.Trivia, 0, 32), new BFCommand(_commandConfig.BeginComment, 0, 33), new BFCommand("a", BFCommandType.Trivia, 0, 39), new BFCommand(_commandConfig.EndComment, 0, 40), new BFCommand(" ", BFCommandType.Trivia, 0, 44) } },
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