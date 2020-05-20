using BFCore.Analyze;
using BFCore.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace brain_fxxk_core.test.Analyze
{
    [TestClass]
    public abstract class AnalyzerTestBase : TestBase
    {
        protected BFAnalyzer _analyzer;

        [TestInitialize]
        public virtual void Init()
        {
            this._analyzer = new BFAnalyzer(new CommonConfig(), new BFCommandConfig());
        }
    }
}
