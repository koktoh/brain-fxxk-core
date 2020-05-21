﻿using System.Collections.Generic;
using System.Linq;
using BFCore.Command;
using BFCore.Config;
using BFCore.Extesion;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace brain_fxxk_core.test.Parse.IgnoreCommentOut
{
    [TestClass]
    public class ParseDefaultCommandCorrectlyTest : IgnoreCommentOutParserTestBase
    {
        private static readonly BFCommandConfig _config = new BFCommandConfig();

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
        [TestCase("#a")]
        [TestCase(" #a")]
        [TestCase("#a ")]
        [TestCase(" #a ")]
        [TestCase("#a;")]
        [TestCase(" #a;")]
        [TestCase("#a; ")]
        [TestCase(" #a; ")]
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
            new object[] { "+", new[] { _config.Increment } },
            new object[] { "-", new[] { _config.Decrement } },
            new object[] { ">", new[] { _config.MoveRight } },
            new object[] { "<", new[] { _config.MoveLeft } },
            new object[] { "[", new[] { _config.LoopHead } },
            new object[] { "]", new[] { _config.LoopTail } },
            new object[] { ",", new[] { _config.Read } },
            new object[] { ".", new[] { _config.Write } },
            new object[] { "#", new[] { new BFCommand("#", BFCommandType.Trivia) } },
            new object[] { ";", new[] { new BFCommand(";", BFCommandType.Trivia) } },
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
            new object[] { "++", new[] { _config.Increment, new BFCommand(_config.Increment, 0, 1) } },
            new object[] { " ++", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 1), new BFCommand(_config.Increment, 0, 2) } },
            new object[] { "+ +", new[] { _config.Increment, new BFCommand(" ", BFCommandType.Trivia, 0, 1), new BFCommand(_config.Increment, 0, 2) } },
            new object[] { "++ ", new[] { _config.Increment, new BFCommand(_config.Increment, 0, 1), new BFCommand(" ", BFCommandType.Trivia, 0, 2) } },
            new object[] { " + +", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 1), new BFCommand(" ", BFCommandType.Trivia, 0, 2), new BFCommand(_config.Increment, 0, 3) } },
            new object[] { "+ + ", new[] { _config.Increment, new BFCommand(" ", BFCommandType.Trivia, 0, 1), new BFCommand(_config.Increment, 0, 2), new BFCommand(" ", BFCommandType.Trivia, 0, 3) } },
            new object[] { " ++ ", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 1), new BFCommand(_config.Increment, 0, 2), new BFCommand(" ", BFCommandType.Trivia, 0, 3) } },
            new object[] { " + + ", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 1), new BFCommand(" ", BFCommandType.Trivia, 0, 2), new BFCommand(_config.Increment, 0, 3), new BFCommand(" ", BFCommandType.Trivia, 0, 4) } },
            new object[] { "#a;++", new[] { new BFCommand("#a;", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 3), new BFCommand(_config.Increment, 0, 4) } },
            new object[] { "+#a;+", new[] { _config.Increment, new BFCommand("#a;", BFCommandType.Trivia, 0, 1), new BFCommand(_config.Increment, 0, 4) } },
            new object[] { "++#a;", new[] { _config.Increment, new BFCommand(_config.Increment, 0, 1), new BFCommand("#a;", BFCommandType.Trivia, 0, 2) } },
            new object[] { "#a;+#a;+", new[] { new BFCommand("#a;", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 3), new BFCommand("#a;", BFCommandType.Trivia, 0, 4), new BFCommand(_config.Increment, 0, 7) } },
            new object[] { "+#a;+#a;", new[] { _config.Increment, new BFCommand("#a;", BFCommandType.Trivia, 0, 1), new BFCommand(_config.Increment, 0, 4), new BFCommand("#a;", BFCommandType.Trivia, 0, 5) } },
            new object[] { "#a;++#a;", new[] { new BFCommand("#a;", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 3), new BFCommand(_config.Increment, 0, 4), new BFCommand("#a;", BFCommandType.Trivia, 0, 5) } },
            new object[] { "#a;+#a;+#a;", new[] { new BFCommand("#a;", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 3), new BFCommand("#a;", BFCommandType.Trivia, 0, 4), new BFCommand(_config.Increment, 0, 7), new BFCommand("#a;", BFCommandType.Trivia, 0, 8) } },
            new object[] { "#a;+ + ", new[] { new BFCommand("#a;", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 3), new BFCommand(" ", BFCommandType.Trivia, 0, 4), new BFCommand(_config.Increment, 0, 5), new BFCommand(" ", BFCommandType.Trivia, 0, 6) } },
            new object[] { " +#a;+ ", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 1), new BFCommand("#a;", BFCommandType.Trivia, 0, 2), new BFCommand(_config.Increment, 0, 5), new BFCommand(" ", BFCommandType.Trivia, 0, 6) } },
            new object[] { " + +#a;", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 1), new BFCommand(" ", BFCommandType.Trivia, 0, 2), new BFCommand(_config.Increment, 0, 3), new BFCommand("#a;", BFCommandType.Trivia, 0, 4) } },
            new object[] { "#a;+#a;+ ", new[] { new BFCommand("#a;", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 3), new BFCommand("#a;", BFCommandType.Trivia, 0, 4), new BFCommand(_config.Increment, 0, 7), new BFCommand(" ", BFCommandType.Trivia, 0, 8) } },
            new object[] { "#a;+ +#a;", new[] { new BFCommand("#a;", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 3), new BFCommand(" ", BFCommandType.Trivia, 0, 4), new BFCommand(_config.Increment, 0, 5), new BFCommand("#a;", BFCommandType.Trivia, 0, 6) } },
            new object[] { " +#a;+#a;", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 1), new BFCommand("#a;", BFCommandType.Trivia, 0, 2), new BFCommand(_config.Increment, 0, 5), new BFCommand("#a;", BFCommandType.Trivia, 0, 6) } },
            new object[] { " + +#a;", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 1), new BFCommand(" ", BFCommandType.Trivia, 0, 2), new BFCommand(_config.Increment, 0, 3), new BFCommand("#a;", BFCommandType.Trivia, 0, 4) } },
            new object[] { " +#a;+ ", new[] { new BFCommand(" ", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 1), new BFCommand("#a;", BFCommandType.Trivia, 0, 2), new BFCommand(_config.Increment, 0, 5), new BFCommand(" ", BFCommandType.Trivia, 0, 6) } },
            new object[] { "#a;+ + ", new[] { new BFCommand("#a;", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 3), new BFCommand(" ", BFCommandType.Trivia, 0, 4), new BFCommand(_config.Increment, 0, 5), new BFCommand(" ", BFCommandType.Trivia, 0, 6) } },
            new object[] { " #a;+#a;+#a;", new[] { new BFCommand(" #a;", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 4), new BFCommand("#a;", BFCommandType.Trivia, 0, 5), new BFCommand(_config.Increment, 0, 8), new BFCommand("#a;", BFCommandType.Trivia, 0, 9) } },
            new object[] { "#a; +#a;+#a;", new[] { new BFCommand("#a; ", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 4), new BFCommand("#a;", BFCommandType.Trivia, 0, 5), new BFCommand(_config.Increment, 0, 8), new BFCommand("#a;", BFCommandType.Trivia, 0, 9) } },
            new object[] { "#a;+ #a;+#a;", new[] { new BFCommand("#a;", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 3), new BFCommand(" #a;", BFCommandType.Trivia, 0, 4), new BFCommand(_config.Increment, 0, 8), new BFCommand("#a;", BFCommandType.Trivia, 0, 9) } },
            new object[] { "#a;+#a; +#a;", new[] { new BFCommand("#a;", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 3), new BFCommand("#a; ", BFCommandType.Trivia, 0, 4), new BFCommand(_config.Increment, 0, 8), new BFCommand("#a;", BFCommandType.Trivia, 0, 9) } },
            new object[] { "#a;+#a;+ #a;", new[] { new BFCommand("#a;", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 3), new BFCommand("#a;", BFCommandType.Trivia, 0, 4), new BFCommand(_config.Increment, 0, 7), new BFCommand(" #a;", BFCommandType.Trivia, 0, 8) } },
            new object[] { "#a;+#a;+#a; ", new[] { new BFCommand("#a;", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 3), new BFCommand("#a;", BFCommandType.Trivia, 0, 4), new BFCommand(_config.Increment, 0, 7), new BFCommand("#a; ", BFCommandType.Trivia, 0, 8) } },
            new object[] { " #a; +#a;+#a;", new[] { new BFCommand(" #a; ", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 5), new BFCommand("#a;", BFCommandType.Trivia, 0, 6), new BFCommand(_config.Increment, 0, 9), new BFCommand("#a;", BFCommandType.Trivia, 0, 10) } },
            new object[] { " #a;+ #a;+#a;", new[] { new BFCommand(" #a;", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 4), new BFCommand(" #a;", BFCommandType.Trivia, 0, 5), new BFCommand(_config.Increment, 0, 9), new BFCommand("#a;", BFCommandType.Trivia, 0, 10) } },
            new object[] { " #a;+#a; +#a;", new[] { new BFCommand(" #a;", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 4), new BFCommand("#a; ", BFCommandType.Trivia, 0, 5), new BFCommand(_config.Increment, 0, 9), new BFCommand("#a;", BFCommandType.Trivia, 0, 10) } },
            new object[] { " #a;+#a;+ #a;", new[] { new BFCommand(" #a;", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 4), new BFCommand("#a;", BFCommandType.Trivia, 0, 5), new BFCommand(_config.Increment, 0, 8), new BFCommand(" #a;", BFCommandType.Trivia, 0, 9) } },
            new object[] { " #a;+#a;+#a; ", new[] { new BFCommand(" #a;", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 4), new BFCommand("#a;", BFCommandType.Trivia, 0, 5), new BFCommand(_config.Increment, 0, 8), new BFCommand("#a; ", BFCommandType.Trivia, 0, 9) } },
            new object[] { " #a; + #a;+#a;", new[] { new BFCommand(" #a; ", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 5), new BFCommand(" #a;", BFCommandType.Trivia, 0, 6), new BFCommand(_config.Increment, 0, 10), new BFCommand("#a;", BFCommandType.Trivia, 0, 11) } },
            new object[] { " #a; +#a; +#a;", new[] { new BFCommand(" #a; ", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 5), new BFCommand("#a; ", BFCommandType.Trivia, 0, 6), new BFCommand(_config.Increment, 0, 10), new BFCommand("#a;", BFCommandType.Trivia, 0, 11) } },
            new object[] { " #a; +#a;+ #a;", new[] { new BFCommand(" #a; ", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 5), new BFCommand("#a;", BFCommandType.Trivia, 0, 6), new BFCommand(_config.Increment, 0, 9), new BFCommand(" #a;", BFCommandType.Trivia, 0, 10) } },
            new object[] { " #a; +#a;+#a; ", new[] { new BFCommand(" #a; ", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 5), new BFCommand("#a;", BFCommandType.Trivia, 0, 6), new BFCommand(_config.Increment, 0, 9), new BFCommand("#a; ", BFCommandType.Trivia, 0, 10) } },
            new object[] { " #a;+ #a; +#a;", new[] { new BFCommand(" #a;", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 4), new BFCommand(" #a; ", BFCommandType.Trivia, 0, 5), new BFCommand(_config.Increment, 0, 10), new BFCommand("#a;", BFCommandType.Trivia, 0, 11) } },
            new object[] { " #a;+ #a;+ #a;", new[] { new BFCommand(" #a;", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 4), new BFCommand(" #a;", BFCommandType.Trivia, 0, 5), new BFCommand(_config.Increment, 0, 9), new BFCommand(" #a;", BFCommandType.Trivia, 0, 10) } },
            new object[] { " #a;+ #a;+#a; ", new[] { new BFCommand(" #a;", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 4), new BFCommand(" #a;", BFCommandType.Trivia, 0, 5), new BFCommand(_config.Increment, 0, 9), new BFCommand("#a; ", BFCommandType.Trivia, 0, 10) } },
            new object[] { " #a;+#a; + #a;", new[] { new BFCommand(" #a;", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 4), new BFCommand("#a; ", BFCommandType.Trivia, 0, 5), new BFCommand(_config.Increment, 0, 9), new BFCommand(" #a;", BFCommandType.Trivia, 0, 10) } },
            new object[] { " #a;+#a; +#a; ", new[] { new BFCommand(" #a;", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 4), new BFCommand("#a; ", BFCommandType.Trivia, 0, 5), new BFCommand(_config.Increment, 0, 9), new BFCommand("#a; ", BFCommandType.Trivia, 0, 10) } },
            new object[] { " #a;+#a;+ #a; ", new[] { new BFCommand(" #a;", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 4), new BFCommand("#a;", BFCommandType.Trivia, 0, 5), new BFCommand(_config.Increment, 0, 8), new BFCommand(" #a; ", BFCommandType.Trivia, 0, 9) } },
            new object[] { " #a; + #a; +#a;", new[] { new BFCommand(" #a; ", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 5), new BFCommand(" #a; ", BFCommandType.Trivia, 0, 6), new BFCommand(_config.Increment, 0, 11), new BFCommand("#a;", BFCommandType.Trivia, 0, 12) } },
            new object[] { " #a; + #a;+ #a;", new[] { new BFCommand(" #a; ", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 5), new BFCommand(" #a;", BFCommandType.Trivia, 0, 6), new BFCommand(_config.Increment, 0, 10), new BFCommand(" #a;", BFCommandType.Trivia, 0, 11) } },
            new object[] { " #a; + #a;+#a; ", new[] { new BFCommand(" #a; ", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 5), new BFCommand(" #a;", BFCommandType.Trivia, 0, 6), new BFCommand(_config.Increment, 0, 10), new BFCommand("#a; ", BFCommandType.Trivia, 0, 11) } },
            new object[] { " #a; + #a; + #a;", new[] { new BFCommand(" #a; ", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 5), new BFCommand(" #a; ", BFCommandType.Trivia, 0, 6), new BFCommand(_config.Increment, 0, 11), new BFCommand(" #a;", BFCommandType.Trivia, 0, 12) } },
            new object[] { " #a; + #a; +#a; ", new[] { new BFCommand(" #a; ", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 5), new BFCommand(" #a; ", BFCommandType.Trivia, 0, 6), new BFCommand(_config.Increment, 0, 11), new BFCommand("#a; ", BFCommandType.Trivia, 0, 12) } },
            new object[] { " #a; + #a; + #a; ", new[] { new BFCommand(" #a; ", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 5), new BFCommand(" #a; ", BFCommandType.Trivia, 0, 6), new BFCommand(_config.Increment, 0, 11), new BFCommand(" #a; ", BFCommandType.Trivia, 0, 12) } },
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
