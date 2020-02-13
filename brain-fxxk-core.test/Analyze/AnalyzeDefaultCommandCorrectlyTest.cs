using System.Collections.Generic;
using System.Linq;
using BFCore.Command;
using BFCore.Config;
using BFCore.Extesion;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace brain_fxxk_core.test.Analyze
{
    [TestClass]
    public class AnalyzeDefaultCommandCorrectlyTest : AnalyzerTestBase
    {
        private static readonly BFCommandConfig _config = new BFCommandConfig();

        [TestMethod]
        [TestCase("")]
        public void EmptyTest()
        {
            this.TestContext.Run<string>(code =>
            {
                this._analyzer.Analyze(code).Count().Is(0);
            });
        }

        [TestMethod]
        [TestCase(" ")]
        [TestCase("#a")]
        [TestCase("#a;")]
        [TestCase("#a+;")]
        [TestCase("#a+")]
        [TestCase(" #a")]
        [TestCase(" #a;")]
        [TestCase(" #a+;")]
        [TestCase(" #a+ ")]
        public void OnlyTriviaTest()
        {
            this.TestContext.Run<string>(code =>
            {
                var analized = this._analyzer.Analyze(code);
                analized.All(x => !x.IsExecutable()).IsTrue($@"TetCase: ""{code}""");
            });
        }

        public static object[] analyzeCorrectlyTestSource = new[]
        {
            new object[] { "+", new[] { _config.Increment } },
            new object[] { "-", new[] { _config.Decrement } },
            new object[] { ">", new[] { _config.MoveRight } },
            new object[] { "<", new[] { _config.MoveLeft } },
            new object[] { "[", new[] { _config.LoopHead } },
            new object[] { "]", new[] { _config.LoopTail } },
            new object[] { ",", new[] { _config.Read } },
            new object[] { ".", new[] { _config.Write } },
            new object[] { "#", new[] { _config.BeginComment } },
            new object[] { ";", new[] { _config.EndComment } },
        };

        [TestMethod]
        [TestCaseSource(nameof(analyzeCorrectlyTestSource))]
        public void AalyzeCorrectlyTest()
        {
            this.TestContext.Run<string, IEnumerable<BFCommand>>((code, expected) =>
            {
                var analized = this._analyzer.Analyze(code);
                analized.Is(expected, $@"TestCase: ""{code}""");
            });
        }

        public static object[] analyzeCorrectlyWithCommentTestSource = new[]
        {
            new object[] { "++", new[] { _config.Increment, _config.Increment } },
            new object[] { " ++", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.Increment, _config.Increment } },
            new object[] { "+ +", new[] { _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment } },
            new object[] { "++ ", new[] { _config.Increment, _config.Increment, new BFCommand(" ", BFCommandType.Trivia) } },
            new object[] { " + +", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment } },
            new object[] { "+ + ", new[] { _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, new BFCommand(" ", BFCommandType.Trivia) } },
            new object[] { " ++ ", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.Increment, _config.Increment, new BFCommand(" ", BFCommandType.Trivia) } },
            new object[] { " + + ", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, new BFCommand(" ", BFCommandType.Trivia) } },
            new object[] { "#a;++", new[] { _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.Increment } },
            new object[] { "+#a;+", new[] { _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment } },
            new object[] { "++#a;", new[] { _config.Increment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { "#a;+#a;+", new[] { _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment } },
            new object[] { "+#a;+#a;", new[] { _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { "#a;++#a;", new[] { _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { "#a;+#a;+#a;", new[] { _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { "#a;+ + ", new[] { _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, new BFCommand(" ", BFCommandType.Trivia) } },
            new object[] { " +#a;+ ", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, new BFCommand(" ", BFCommandType.Trivia), } },
            new object[] { " + +#a;", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { "#a;+#a;+ ", new[] { _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, new BFCommand(" ", BFCommandType.Trivia) } },
            new object[] { "#a;+ +#a;", new[] { _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { " +#a;+#a;", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { " + +#a;", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { " +#a;+ ", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, new BFCommand(" ", BFCommandType.Trivia) } },
            new object[] { "#a;+ + ", new[] { _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, new BFCommand(" ", BFCommandType.Trivia) } },
            new object[] { " #a;+#a;+#a;", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { "#a; +#a;+#a;", new[] { _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { "#a;+ #a;+#a;", new[] { _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { "#a;+#a; +#a;", new[] { _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { "#a;+#a;+ #a;", new[] { _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { "#a;+#a;+#a; ", new[] { _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia) } },
            new object[] { " #a; +#a;+#a;", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { " #a;+ #a;+#a;", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { " #a;+#a; +#a;", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { " #a;+#a;+ #a;", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { " #a;+#a;+#a; ", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia) } },
            new object[] { " #a; + #a;+#a;", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { " #a; +#a; +#a;", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { " #a; +#a;+ #a;", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { " #a; +#a;+#a; ", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia) } },
            new object[] { " #a;+ #a; +#a;", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { " #a;+ #a;+ #a;", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { " #a;+ #a;+#a; ", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia) } },
            new object[] { " #a;+#a; + #a;", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { " #a;+#a; +#a; ", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia) } },
            new object[] { " #a;+#a;+ #a; ", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia) } },
            new object[] { " #a; + #a; +#a;", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { " #a; + #a;+ #a;", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { " #a; + #a;+#a; ", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia) } },
            new object[] { " #a; + #a; + #a;", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { " #a; + #a; +#a; ", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia) } },
            new object[] { " #a; + #a; + #a; ", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia) } },
        };

        [TestMethod]
        [TestCaseSource(nameof(analyzeCorrectlyWithCommentTestSource))]
        public void AnalyzeCorrectlyWithCommentTest()
        {
            this.TestContext.Run<string, IEnumerable<BFCommand>>((code, expected) =>
            {
                var analized = this._analyzer.Analyze(code);
                analized.Is(expected, $@"TestCase: ""{code}""");
            });
        }
    }
}
