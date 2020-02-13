using BFCore.Config;
using BFCore.Exception;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace brain_fxxk_core.test.Execute
{
    [TestClass]
    public class ExecuterErrorTest : TestBase
    {
        private TestExecuter _executer;

        [TestInitialize]
        public void Init()
        {
            var config = new CommonConfig();
            var commandConfig = new BFCommandConfig();
            this._executer = new TestExecuter(config, commandConfig);
        }

        [TestMethod]
        [TestCase("++[++")]
        [TestCase("++]++")]
        public void ThrowsBFSyntaxErrorTest()
        {
            AssertEx.Throws<BFSyntaxException>(()=> 
            {
                this.TestContext.Run<string>(code => this._executer.Execute(code));
            });
        }
    }
}
