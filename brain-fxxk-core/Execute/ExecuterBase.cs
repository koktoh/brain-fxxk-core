using System.Collections.Generic;
using System.IO;
using System.Linq;
using BFCore.Analyze;
using BFCore.Command;
using BFCore.Config;
using BFCore.Extesion;

namespace BFCore.Execute
{
    public abstract class ExecuterBase
    {
        private readonly CommonConfig _config;
        private readonly BFAnalyzer _analyzer;

        protected int _index;
        protected int[] _memory;

        protected ExecuterBase(CommonConfig config, BFCommandConfig commandConfig)
        {
            this._config = config;
            this._analyzer = new BFAnalyzer(commandConfig);
        }

        private void Init()
        {
            this._index = 0;
            this._memory = new int[this._config.MemorySize];
        }

        public void Execute(Stream stream)
        {
            this.Init();

            var commands = this._analyzer.Analyze(stream);
            this.ExecuteCommands(commands);
        }

        public void Execute(string code)
        {
            this.Init();

            var commands = this._analyzer.Analyze(code);
            this.ExecuteCommands(commands);
        }

        private void ExecuteCommands(IEnumerable<BFCommand> commands)
        {
            var executableCommands = commands.Where(x => x.IsExecutable()).ToList();

            for (int i = 0; i < executableCommands.Count; i++)
            {
                var command = executableCommands[i];

                switch (command.CommandType)
                {
                    case BFCommandType.Increment:
                        this._memory[this._index]++;
                        break;
                    case BFCommandType.Decrement:
                        this._memory[this._index]--;
                        break;
                    case BFCommandType.MoveRight:
                        this._index++;
                        break;
                    case BFCommandType.MoveLeft:
                        this._index--;
                        break;
                    case BFCommandType.LoopHead:
                        if (this._memory[this._index] == 0)
                        {
                            while (i < executableCommands.Count())
                            {
                                if (executableCommands[++i].IsLoopTail())
                                {
                                    break;
                                }
                            }
                        }
                        break;
                    case BFCommandType.LoopTail:
                        if (this._memory[this._index] != 0)
                        {
                            while (i >= 0)
                            {
                                if (executableCommands[--i].IsLoopHead())
                                {
                                    break;
                                }
                            }
                        }
                        break;
                    case BFCommandType.Read:
                        this.Read();
                        break;
                    case BFCommandType.Write:
                        this.Write();
                        break;
                    default:
                        break;
                }

            }
        }

        protected abstract void Read();

        protected abstract void Write();
    }
}
