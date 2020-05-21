using System.Collections.Generic;
using System.IO;
using System.Linq;
using BFCore.Analyze;
using BFCore.Command;
using BFCore.Config;
using BFCore.Exception;
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
                        try
                        {
                            this._memory[this._index]++;
                        }
                        catch (System.Exception e)
                        {
                            throw new BFRuntimeException(e.Message, e);
                        }
                        break;
                    case BFCommandType.Decrement:
                        try
                        {
                            this._memory[this._index]--;
                        }
                        catch (System.Exception e)
                        {
                            throw new BFRuntimeException(e.Message, e);
                        }
                        break;
                    case BFCommandType.MoveRight:
                        this._index++;

                        if (this._index > this._config.MemorySize)
                        {
                            throw new BFRuntimeException("Index was outside the bounds of the array.");
                        }
                        break;
                    case BFCommandType.MoveLeft:
                        this._index--;

                        if (this._index < 0)
                        {
                            throw new BFRuntimeException("Index was outside the bounds of the array.");
                        }
                        break;
                    case BFCommandType.LoopHead:
                        if (this._memory[this._index] == 0)
                        {
                            if (!this.TryGoToLoopTail(executableCommands, ref i))
                            {
                                throw new BFSyntaxException("Does not have paired Loop Tail Command");
                            }
                        }
                        break;
                    case BFCommandType.LoopTail:
                        if (this._memory[this._index] != 0)
                        {
                            if (!this.TryGoToLoopHead(executableCommands, ref i))
                            {
                                throw new BFSyntaxException("Does not have paired Loop Head Command");
                            }
                        }
                        break;
                    case BFCommandType.Read:
                        try
                        {
                            this.Read();
                        }
                        catch (System.Exception e)
                        {
                            throw new BFRuntimeException(e.Message, e);
                        }
                        break;
                    case BFCommandType.Write:
                        try
                        {
                            this.Write();
                        }
                        catch (System.Exception e)
                        {
                            throw new BFRuntimeException(e.Message, e);
                        }
                        break;
                    default:
                        break;
                }

            }
        }

        private bool TryGoToLoopHead(IList<BFCommand> commands, ref int index)
        {
            if (index <= 0)
            {
                return false;
            }

            index--;

            var toFindCount = 1;
            for (; index >= 0; index--)
            {
                var command = commands[index];

                if (command.IsLoopHead())
                {
                    toFindCount--;
                }

                if (command.IsLoopTail())
                {
                    toFindCount++;
                }

                if (toFindCount == 0)
                {
                    return true;
                }
            }

            return false;
        }

        private bool TryGoToLoopTail(IList<BFCommand> commands, ref int index)
        {
            if (index >= commands.Count)
            {
                return false;
            }

            index++;

            var toFindCount = 1;
            for (; index < commands.Count; index++)
            {
                var command = commands[index];

                if (command.IsLoopHead())
                {
                    toFindCount++;
                }

                if (command.IsLoopTail())
                {
                    toFindCount--;
                }

                if (toFindCount == 0)
                {
                    return true;
                }
            }

            return false;
        }

        protected abstract void Read();

        protected abstract void Write();
    }
}
