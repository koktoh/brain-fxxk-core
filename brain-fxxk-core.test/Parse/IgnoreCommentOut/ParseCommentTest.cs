using System.Collections.Generic;
using System.Linq;
using BFCore.Command;
using BFCore.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace brain_fxxk_core.test.Parse.IgnoreCommentOut
{
    [TestClass]
    public class ParseCommentTest:IgnoreCommentOutParserTestBase
    {
        private static readonly BFCommandConfig _config = new BFCommandConfig();

        [TestMethod]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("#a")]
        [TestCase("#a;")]
        [TestCase("#a+;")]
        [TestCase(" #a")]
        [TestCase(" #a;")]
        [TestCase(" #a+;")]
        [TestCase("++")]
        [TestCase(" ++")]
        [TestCase("+ +")]
        [TestCase("++ ")]
        [TestCase(" + +")]
        [TestCase("+ + ")]
        [TestCase(" ++ ")]
        [TestCase(" + + ")]
        [TestCase("#a;++")]
        [TestCase("+#a;+")]
        [TestCase("++#a;")]
        [TestCase("#a;+#a;+")]
        [TestCase("+#a;+#a;")]
        [TestCase("#a;++#a;")]
        [TestCase("#a;+#a;+#a;")]
        [TestCase("#a;+ + ")]
        [TestCase(" +#a;+ ")]
        [TestCase(" + +#a;")]
        [TestCase("#a;+#a;+ ")]
        [TestCase("#a;+ +#a;")]
        [TestCase(" +#a;+#a;")]
        [TestCase(" + +#a;")]
        [TestCase(" +#a;+ ")]
        [TestCase("#a;+ + ")]
        [TestCase(" #a;+#a;+#a;")]
        [TestCase("#a; +#a;+#a;")]
        [TestCase("#a;+ #a;+#a;")]
        [TestCase("#a;+#a; +#a;")]
        [TestCase("#a;+#a;+ #a;")]
        [TestCase("#a;+#a;+#a; ")]
        [TestCase(" #a; +#a;+#a;")]
        [TestCase(" #a;+ #a;+#a;")]
        [TestCase(" #a;+#a; +#a;")]
        [TestCase(" #a;+#a;+ #a;")]
        [TestCase(" #a;+#a;+#a; ")]
        [TestCase(" #a; + #a;+#a;")]
        [TestCase(" #a; +#a; +#a;")]
        [TestCase(" #a; +#a;+ #a;")]
        [TestCase(" #a; +#a;+#a; ")]
        [TestCase(" #a;+ #a; +#a;")]
        [TestCase(" #a;+ #a;+ #a;")]
        [TestCase(" #a;+ #a;+#a; ")]
        [TestCase(" #a;+#a; + #a;")]
        [TestCase(" #a;+#a; +#a; ")]
        [TestCase(" #a;+#a;+ #a; ")]
        [TestCase(" #a; + #a; +#a;")]
        [TestCase(" #a; + #a;+ #a;")]
        [TestCase(" #a; + #a;+#a; ")]
        [TestCase(" #a; + #a; + #a;")]
        [TestCase(" #a; + #a; +#a; ")]
        [TestCase(" #a; + #a; + #a; ")]
        public void DoesNotThrowExceptionTest()
        {
            this.TestContext.Run<string>(code =>
            {
                AssertEx.DoesNotThrow(() =>
                {
                    this._parser.Parse(code);
                },
                $@"TestCase: ""{code}""");
            });
        }

        public static object[] inLineCommandWithinCommandsTestSource = new object[]
        {
            new object[] { "#+;", new[] { new BFCommand("#", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 1), new BFCommand(";", BFCommandType.Trivia, 0, 2) } },
            new object[] { "#-;", new[] { new BFCommand("#", BFCommandType.Trivia), new BFCommand(_config.Decrement, 0, 1), new BFCommand(";", BFCommandType.Trivia, 0, 2) } },
            new object[] { "#>;", new[] { new BFCommand("#", BFCommandType.Trivia), new BFCommand(_config.MoveRight, 0, 1), new BFCommand(";", BFCommandType.Trivia, 0, 2) } },
            new object[] { "#<;", new[] { new BFCommand("#", BFCommandType.Trivia), new BFCommand(_config.MoveLeft, 0, 1), new BFCommand(";", BFCommandType.Trivia, 0, 2) } },
            new object[] { "#[;", new[] { new BFCommand("#", BFCommandType.Trivia), new BFCommand(_config.LoopHead, 0, 1), new BFCommand(";", BFCommandType.Trivia, 0, 2) } },
            new object[] { "#];", new[] { new BFCommand("#", BFCommandType.Trivia), new BFCommand(_config.LoopTail, 0, 1), new BFCommand(";", BFCommandType.Trivia, 0, 2) } },
            new object[] { "#,;", new[] { new BFCommand("#", BFCommandType.Trivia), new BFCommand(_config.Read, 0, 1), new BFCommand(";", BFCommandType.Trivia, 0, 2) } },
            new object[] { "#.;", new[] { new BFCommand("#", BFCommandType.Trivia), new BFCommand(_config.Write, 0, 1), new BFCommand(";", BFCommandType.Trivia, 0, 2) } },
            new object[] { "##;", new[] { new BFCommand("##;", BFCommandType.Trivia) } },
            new object[] { "#+", new[] { new BFCommand("#", BFCommandType.Trivia), new BFCommand(_config.Increment, 0, 1) } },
            new object[] { "#-", new[] { new BFCommand("#", BFCommandType.Trivia), new BFCommand(_config.Decrement, 0, 1) } },
            new object[] { "#>", new[] { new BFCommand("#", BFCommandType.Trivia), new BFCommand(_config.MoveRight, 0, 1) } },
            new object[] { "#<", new[] { new BFCommand("#", BFCommandType.Trivia), new BFCommand(_config.MoveLeft, 0, 1) } },
            new object[] { "#[", new[] { new BFCommand("#", BFCommandType.Trivia), new BFCommand(_config.LoopHead, 0, 1) } },
            new object[] { "#]", new[] { new BFCommand("#", BFCommandType.Trivia), new BFCommand(_config.LoopTail, 0, 1) } },
            new object[] { "#,", new[] { new BFCommand("#", BFCommandType.Trivia), new BFCommand(_config.Read, 0, 1) } },
            new object[] { "#.", new[] { new BFCommand("#", BFCommandType.Trivia), new BFCommand(_config.Write, 0, 1) } },
            new object[] { "##", new[] { new BFCommand("##", BFCommandType.Trivia) } },
        };

        [TestMethod]
        [TestCaseSource(nameof(inLineCommandWithinCommandsTestSource))]
        public void InLineCommentWithinCommandsTest()
        {
            this.TestContext.Run<string, IEnumerable<BFCommand>>((code, expected) =>
            {
                var parsed = this._parser.Parse(code).ToArray();
                parsed.Is(expected, $@"TestCase: ""{code}""");
            });
        }

    }
}
