using BFCore.Config;
using BFCore.Parse;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace brain_fxxk_core.test.Parse.IgnoreCommentOut
{
    [TestClass]
    public abstract class IgnoreCommentOutParserTestBase : TestBase
    {
        internal IParser _parser;

        protected virtual BFCommandConfig CommandConfig => new BFCommandConfig();

        [TestInitialize]
        public virtual void Init()
        {
            var factory = new ParserFactory(CommandConfig);
            this._parser = factory.Create(false);
        }
    }
}
