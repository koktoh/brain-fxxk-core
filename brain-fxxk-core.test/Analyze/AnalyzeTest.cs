using System;
using System.Collections.Generic;
using System.Text;
using BFCore.Exception;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace brain_fxxk_core.test.Analyze
{
    [TestClass]
    public class AnalyzeTest : AnalyzerTestBase
    {
        [TestMethod]
        [TestCase("[]")]
        [TestCase("[[]]")]
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

        [TestMethod]
        [TestCase("[")]
        [TestCase("]")]
        [TestCase("[[]")]
        [TestCase("[]]")]
        public void ThrowExceptionTest()
        {
            this.TestContext.Run<string>(code =>
            {
                AssertEx.Throws<BFSyntaxException>(() =>
                {
                    this._analyzer.Analyze(code);
                },
                $@"TestCase: ""{code}""");
            });
        }
    }
}
