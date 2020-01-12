using System.Collections.Generic;
using System.Linq;
using BFCore.Analyze;
using BFCore.Command;
using BFCore.Config;
using BFCore.Extesion;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace brain_fxxk_core.test.Analyze
{
    [TestClass]
    public class AnalyzeCustomCommandCorrectlyTest : AnalyzerTestBase
    {
        private static readonly BFCommandConfig _config = new BFCommandConfig
        {
            Increment = new BFCommand("inc", BFCommandType.Increment),
            Decrement = new BFCommand("dec", BFCommandType.Decrement),
            MoveRight = new BFCommand("toR", BFCommandType.MoveRight),
            MoveLeft = new BFCommand("toL", BFCommandType.MoveLeft),
            RoopHead = new BFCommand("roopH", BFCommandType.RoopHead),
            RoopTale = new BFCommand("roopT", BFCommandType.RoopTale),
            Read = new BFCommand("read", BFCommandType.Read),
            Write = new BFCommand("write", BFCommandType.Write),
            BeginComment = new BFCommand("beginC", BFCommandType.BeginComment),
            EndComment = new BFCommand("endC", BFCommandType.EndComment),
        };

        [TestInitialize]
        public override void Init()
        {
            this._analyzer = new BFAnalyzer(_config);
        }

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
                var analized = this._analyzer.Analyze(code);
                analized.All(x => !x.IsExecutable()).IsTrue($@"TetCase: ""{code}""");
            });
        }

        public static object[] analyzeCorrectlyTestSource = new[]
        {
            new object[] { "inc", new[] { _config.Increment } },
            new object[] { "dec", new[] { _config.Decrement } },
            new object[] { "toR", new[] { _config.MoveRight } },
            new object[] { "toL", new[] { _config.MoveLeft } },
            new object[] { "roopH", new[] { _config.RoopHead } },
            new object[] { "roopT", new[] { _config.RoopTale } },
            new object[] { "read", new[] { _config.Read } },
            new object[] { "write", new[] { _config.Write } },
            new object[] { "beginC", new[] { _config.BeginComment } },
            new object[] { "endC", new[] { _config.EndComment } },
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
            new object[] { "incinc", new[] { _config.Increment, _config.Increment } },
            new object[] { " incinc", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.Increment, _config.Increment } },
            new object[] { "inc inc", new[] { _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment } },
            new object[] { "incinc ", new[] { _config.Increment, _config.Increment, new BFCommand(" ", BFCommandType.Trivia) } },
            new object[] { " inc inc", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment } },
            new object[] { "inc inc ", new[] { _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, new BFCommand(" ", BFCommandType.Trivia) } },
            new object[] { " incinc ", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.Increment, _config.Increment, new BFCommand(" ", BFCommandType.Trivia) } },
            new object[] { " inc inc ", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, new BFCommand(" ", BFCommandType.Trivia) } },
            new object[] { "beginCaendCincinc", new[] { _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.Increment } },
            new object[] { "incbeginCaendCinc", new[] { _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment } },
            new object[] { "incincbeginCaendC", new[] { _config.Increment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { "beginCaendCincbeginCaendCinc", new[] { _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment } },
            new object[] { "incbeginCaendCincbeginCaendC", new[] { _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { "beginCaendCincincbeginCaendC", new[] { _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { "beginCaendCincbeginCaendCincbeginCaendC", new[] { _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { "beginCaendCinc inc ", new[] { _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, new BFCommand(" ", BFCommandType.Trivia) } },
            new object[] { " incbeginCaendCinc ", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, new BFCommand(" ", BFCommandType.Trivia), } },
            new object[] { " inc incbeginCaendC", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { "beginCaendCincbeginCaendCinc ", new[] { _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, new BFCommand(" ", BFCommandType.Trivia) } },
            new object[] { "beginCaendCinc incbeginCaendC", new[] { _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { " incbeginCaendCincbeginCaendC", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { " inc incbeginCaendC", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { " incbeginCaendCinc ", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, new BFCommand(" ", BFCommandType.Trivia) } },
            new object[] { "beginCaendCinc inc ", new[] { _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, new BFCommand(" ", BFCommandType.Trivia) } },
            new object[] { " beginCaendCincbeginCaendCincbeginCaendC", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { "beginCaendC incbeginCaendCincbeginCaendC", new[] { _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { "beginCaendCinc beginCaendCincbeginCaendC", new[] { _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { "beginCaendCincbeginCaendC incbeginCaendC", new[] { _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { "beginCaendCincbeginCaendCinc beginCaendC", new[] { _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { "beginCaendCincbeginCaendCincbeginCaendC ", new[] { _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia) } },
            new object[] { " beginCaendC incbeginCaendCincbeginCaendC", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { " beginCaendCinc beginCaendCincbeginCaendC", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { " beginCaendCincbeginCaendC incbeginCaendC", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { " beginCaendCincbeginCaendCinc beginCaendC", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { " beginCaendCincbeginCaendCincbeginCaendC ", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia) } },
            new object[] { " beginCaendC inc beginCaendCincbeginCaendC", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { " beginCaendC incbeginCaendC incbeginCaendC", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { " beginCaendC incbeginCaendCinc beginCaendC", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { " beginCaendC incbeginCaendCincbeginCaendC ", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia) } },
            new object[] { " beginCaendCinc beginCaendC incbeginCaendC", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { " beginCaendCinc beginCaendCinc beginCaendC", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { " beginCaendCinc beginCaendCincbeginCaendC ", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia) } },
            new object[] { " beginCaendCincbeginCaendC inc beginCaendC", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { " beginCaendCincbeginCaendC incbeginCaendC ", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia) } },
            new object[] { " beginCaendCincbeginCaendCinc beginCaendC ", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia) } },
            new object[] { " beginCaendC inc beginCaendC incbeginCaendC", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { " beginCaendC inc beginCaendCinc beginCaendC", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { " beginCaendC inc beginCaendCincbeginCaendC ", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia) } },
            new object[] { " beginCaendC inc beginCaendC inc beginCaendC", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment } },
            new object[] { " beginCaendC inc beginCaendC incbeginCaendC ", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia) } },
            new object[] { " beginCaendC inc beginCaendC inc beginCaendC ", new[] { new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia), _config.Increment, new BFCommand(" ", BFCommandType.Trivia), _config.BeginComment, new BFCommand("a", BFCommandType.Trivia), _config.EndComment, new BFCommand(" ", BFCommandType.Trivia) } },
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

