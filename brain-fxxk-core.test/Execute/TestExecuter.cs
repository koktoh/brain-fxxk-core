using BFCore.Config;
using BFCore.Execute;

namespace brain_fxxk_core.test.Execute
{
    public class TestExecuter : ExecuterBase
    {
        public string Text { get; set; }

        public TestExecuter(CommonConfig config, BFCommandConfig commandConfig) : base(config, commandConfig)
        {
        }

        protected override void Read()
        {
        }

        protected override void Write()
        {
            this.Text += (char)this._memory[this._index];
        }
    }
}

