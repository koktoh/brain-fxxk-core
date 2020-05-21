using BFCore.Config;
using BFCore.Parse;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace brain_fxxk_core.test.Parse.Default
{
    [TestClass]
    public abstract class DefaultParserTestBase : TestBase
    {
        internal IParser _parser;

        protected virtual BFCommandConfig CommandConfig => new BFCommandConfig();

        [TestInitialize]
        public virtual void Init()
        {
            var factory = new ParserFactory(CommandConfig);
            this._parser = factory.Create(true);
        }
    }
}
