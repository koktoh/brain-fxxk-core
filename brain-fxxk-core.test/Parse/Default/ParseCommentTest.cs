using System.Collections.Generic;
using System.Linq;
using BFCore.Command;
using BFCore.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace brain_fxxk_core.test.Parse.Default
{
    [TestClass]
    public class ParseCommentTest : DefaultParserTestBase
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
            new object[] { "#+;", new[] { _config.BeginComment, new BFCommand("+", BFCommandType.Trivia, 0, 1), new BFCommand(_config.EndComment, 0, 2) } },
            new object[] { "#-;", new[] { _config.BeginComment, new BFCommand("-", BFCommandType.Trivia, 0, 1), new BFCommand(_config.EndComment, 0, 2) } },
            new object[] { "#>;", new[] { _config.BeginComment, new BFCommand(">", BFCommandType.Trivia, 0, 1), new BFCommand(_config.EndComment, 0, 2) } },
            new object[] { "#<;", new[] { _config.BeginComment, new BFCommand("<", BFCommandType.Trivia, 0, 1), new BFCommand(_config.EndComment, 0, 2) } },
            new object[] { "#[;", new[] { _config.BeginComment, new BFCommand("[", BFCommandType.Trivia, 0, 1), new BFCommand(_config.EndComment, 0, 2) } },
            new object[] { "#];", new[] { _config.BeginComment, new BFCommand("]", BFCommandType.Trivia, 0, 1), new BFCommand(_config.EndComment, 0, 2) } },
            new object[] { "#,;", new[] { _config.BeginComment, new BFCommand(",", BFCommandType.Trivia, 0, 1), new BFCommand(_config.EndComment, 0, 2) } },
            new object[] { "#.;", new[] { _config.BeginComment, new BFCommand(".", BFCommandType.Trivia, 0, 1), new BFCommand(_config.EndComment, 0, 2) } },
            new object[] { "##;", new[] { _config.BeginComment, new BFCommand("#", BFCommandType.Trivia, 0, 1), new BFCommand(_config.EndComment, 0, 2) } },
            new object[] { "#+", new[] { _config.BeginComment, new BFCommand("+", BFCommandType.Trivia, 0, 1) } },
            new object[] { "#-", new[] { _config.BeginComment, new BFCommand("-", BFCommandType.Trivia, 0, 1) } },
            new object[] { "#>", new[] { _config.BeginComment, new BFCommand(">", BFCommandType.Trivia, 0, 1) } },
            new object[] { "#<", new[] { _config.BeginComment, new BFCommand("<", BFCommandType.Trivia, 0, 1) } },
            new object[] { "#[", new[] { _config.BeginComment, new BFCommand("[", BFCommandType.Trivia, 0, 1) } },
            new object[] { "#]", new[] { _config.BeginComment, new BFCommand("]", BFCommandType.Trivia, 0, 1) } },
            new object[] { "#,", new[] { _config.BeginComment, new BFCommand(",", BFCommandType.Trivia, 0, 1) } },
            new object[] { "#.", new[] { _config.BeginComment, new BFCommand(".", BFCommandType.Trivia, 0, 1) } },
            new object[] { "##", new[] { _config.BeginComment, new BFCommand("#", BFCommandType.Trivia, 0, 1) } },
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
