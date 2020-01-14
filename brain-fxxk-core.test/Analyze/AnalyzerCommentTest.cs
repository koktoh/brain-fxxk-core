using System.Collections.Generic;
using BFCore.Command;
using BFCore.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace brain_fxxk_core.test.Analyze
{
    [TestClass]
    public class AnalyzerCommentTest : AnalyzerTestBase
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
                    this._analyzer.Analyze(code);
                },
                $@"TestCase: ""{code}""");
            });
        }

        public static object[] inLineCommandWithinCommandsTestSource = new object[]
        {
            new object[] { "#+;", new[] { _config.BeginComment, new BFCommand("+",BFCommandType.Trivia), _config.EndComment } },
            new object[] { "#-;", new[] { _config.BeginComment, new BFCommand("-",BFCommandType.Trivia), _config.EndComment } },
            new object[] { "#>;", new[] { _config.BeginComment, new BFCommand(">",BFCommandType.Trivia), _config.EndComment } },
            new object[] { "#<;", new[] { _config.BeginComment, new BFCommand("<",BFCommandType.Trivia), _config.EndComment } },
            new object[] { "#[;", new[] { _config.BeginComment, new BFCommand("[",BFCommandType.Trivia), _config.EndComment } },
            new object[] { "#];", new[] { _config.BeginComment, new BFCommand("]",BFCommandType.Trivia), _config.EndComment } },
            new object[] { "#,;", new[] { _config.BeginComment, new BFCommand(",",BFCommandType.Trivia), _config.EndComment } },
            new object[] { "#.;", new[] { _config.BeginComment, new BFCommand(".",BFCommandType.Trivia), _config.EndComment } },
            new object[] { "##;", new[] { _config.BeginComment, new BFCommand("#",BFCommandType.Trivia), _config.EndComment } },
            new object[] { "#+", new[] { _config.BeginComment, new BFCommand("+",BFCommandType.Trivia) } },
            new object[] { "#-", new[] { _config.BeginComment, new BFCommand("-",BFCommandType.Trivia) } },
            new object[] { "#>", new[] { _config.BeginComment, new BFCommand(">",BFCommandType.Trivia) } },
            new object[] { "#<", new[] { _config.BeginComment, new BFCommand("<",BFCommandType.Trivia) } },
            new object[] { "#[", new[] { _config.BeginComment, new BFCommand("[",BFCommandType.Trivia) } },
            new object[] { "#]", new[] { _config.BeginComment, new BFCommand("]",BFCommandType.Trivia) } },
            new object[] { "#,", new[] { _config.BeginComment, new BFCommand(",",BFCommandType.Trivia) } },
            new object[] { "#.", new[] { _config.BeginComment, new BFCommand(".",BFCommandType.Trivia) } },
            new object[] { "##", new[] { _config.BeginComment, new BFCommand("#",BFCommandType.Trivia) } },
        };

        [TestMethod]
        [TestCaseSource(nameof(inLineCommandWithinCommandsTestSource))]
        public void InLineCommentWithinCommandsTest()
        {
            this.TestContext.Run<string, IEnumerable<BFCommand>>((code, expected) =>
            {
                var analized= this._analyzer.Analyze(code);
                analized.Is(expected, $@"TestCase: ""{code}""");
            });
        }
    }
}
