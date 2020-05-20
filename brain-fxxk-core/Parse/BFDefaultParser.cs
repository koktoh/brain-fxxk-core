using System.Collections.Generic;
using System.Linq;
using System.Text;
using BFCore.Command;
using BFCore.Config;
using BFCore.Extesion;

namespace BFCore.Parse
{
    internal class BFDefaultParser : ParserBase
    {
        private readonly IReadOnlyList<BFCommand> _definedCommands;

        public BFDefaultParser(BFCommandConfig config)
        {
            this._definedCommands = config.GetCommands().ToList();
        }

        private bool TryGetNextCommand(string row, int currentPos, out BFCommand command)
        {
            command = this._definedCommands
                .FirstOrDefault(x => row.Substring(currentPos).StartsWith(x));

            if (command.IsDefinedCommand())
            {
                command = new BFCommand(command, this._currentRowPos, currentPos);

                return true;
            }

            return false;
        }

        private BFCommand GetTrivia(string row, int currentPos, bool inComment)
        {
            var length = 0;

            for (; currentPos + length < row.Length; length++)
            {
                var isCommand = this.TryGetNextCommand(row, currentPos + length, out var command);

                if ((inComment && isCommand && command.IsEndComment())
                    || (!inComment && isCommand))
                {
                    break;
                }
            }

            return new BFCommand(row.Substring(currentPos, length),
                BFCommandType.Trivia,
                this._currentRowPos,
                currentPos);
        }

        protected override IEnumerable<BFCommand> ParseByRow(string row)
        {
            BFCommand prevCommand = default;

            for (int currentPos = 0; currentPos < row.Length;)
            {
                if (this.TryGetNextCommand(row, currentPos, out var command) && !prevCommand.IsBeginComment())
                {
                    yield return command;
                    prevCommand = command;
                    currentPos += command.Length;
                    continue;
                }
                else
                {
                    command = this.GetTrivia(row, currentPos, prevCommand.IsBeginComment());
                    yield return command;
                    prevCommand = command;
                    currentPos += command.Length;
                    continue;
                }
            }
        }
    }
}
