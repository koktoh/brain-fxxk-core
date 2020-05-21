using System.Collections.Generic;
using System.Linq;
using BFCore.Command;
using BFCore.Config;
using BFCore.Extesion;

namespace BFCore.Parse
{
    internal class BFIgnoreCommentOutParser : ParserBase
    {
        private readonly IReadOnlyList<BFCommand> _definedCommands;

        public BFIgnoreCommentOutParser(BFCommandConfig config)
        {
            this._definedCommands = config.GetCommands().ToList();
        }

        private bool TryGetNextExecutableCommand(string row, int currentPos, out BFCommand command)
        {
            command = this._definedCommands
                .FirstOrDefault(x => row.Substring(currentPos).StartsWith(x));

            if (command.IsExecutable())
            {
                command = new BFCommand(command, this._currentRowPos, currentPos);

                return true;
            }

            return false;
        }

        private BFCommand GetTrivia(string row, int currentPos)
        {
            var length = 0;

            for (; currentPos + length < row.Length; length++)
            {
                if (this.TryGetNextExecutableCommand(row, currentPos + length, out _))
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
            for (int currentPos = 0; currentPos < row.Length;)
            {
                if (this.TryGetNextExecutableCommand(row, currentPos, out var command))
                {
                    yield return command;
                    currentPos += command.Length;
                    continue;
                }
                else
                {
                    command = this.GetTrivia(row, currentPos);
                    yield return command;
                    currentPos += command.Length;
                    continue;
                }
            }
        }
    }
}
