using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace brain_fxxk_core.test.Analyze
{
    [TestClass]
    public class AnalyzerCommentTest : AnalyzerTestBase
    {
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
    }
}
