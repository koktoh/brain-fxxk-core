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
        private readonly BFAnalyzer _analyzer;

        protected int _index = 0;
        protected int[] _memory = new int[2024];

        protected ExecuterBase(BFCommandConfig config)
        {
            this._analyzer = new BFAnalyzer(config);
        }

        public void Execute(Stream stream)
        {
            var commands = this._analyzer.Analyze(stream);
            this.ExecuteCommands(commands);
        }

        public void Execute(string code)
        {
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
                    case BFCommandType.RoopHead:
                        if (this._memory[this._index] == 0)
                        {
                            while (i < executableCommands.Count())
                            {
                                if (executableCommands[++i].IsRoopTale())
                                {
                                    break;
                                }
                            }
                        }
                        break;
                    case BFCommandType.RoopTale:
                        if (this._memory[this._index] != 0)
                        {
                            while (i >= 0)
                            {
                                if (executableCommands[--i].IsRoopHead())
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
