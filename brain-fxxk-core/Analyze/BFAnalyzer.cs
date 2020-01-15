using System.Collections.Generic;
using System.IO;
using System.Linq;
using BFCore.Command;
using BFCore.Config;
using BFCore.Exception;
using BFCore.Extesion;

namespace BFCore.Analyze
{
    public class BFAnalyzer
    {
        private readonly BFCommandConfig _config;
        private readonly IReadOnlyList<BFCommand> _definedCommands;

        public BFAnalyzer(BFCommandConfig config)
        {
            this._config = config;

            this._definedCommands = config
                .GetType()
                .GetProperties()
                .Select(x => (BFCommand)x.GetValue(this._config))
                .ToList();
        }

        public IEnumerable<BFCommand> Analyze(Stream stream)
        {
            using var sr = new StreamReader(stream);
            return this.Analyze(sr.ReadToEnd());
        }

        public IEnumerable<BFCommand> Analyze(string code)
        {
            return this.AnalyzeCode(code);
        }

        private bool TryGetNextTargetCommand(string code, BFCommand targetCommand, out int index)
        {
            index = 0;

            for (int i = 0; i < code.Length;)
            {
                var c = code[i].ToString();

                if (targetCommand.Command.StartsWith(c) && targetCommand.Length <= code.Length - i && targetCommand.Command.Equals(code.Substring(i, targetCommand.Length)))
                {
                    index = i;
                    return true;
                }

                i++;
            }

            return false;
        }

        private bool TryGetNextCommand(string code, out BFCommand command, out int length)
        {
            length = 0;
            command = new BFCommand();

            for (int i = 0; i < code.Length;)
            {
                var c = code[i].ToString();

                command = this._definedCommands
                    .Where(x => x.Command.StartsWith(c))
                    .Where(x => x.Length <= code.Length - i)
                    .FirstOrDefault(x => x.Command.Equals(code.Substring(i, x.Length)));

                if (command.IsDefinedCommand())
                {
                    length = i;
                    return true;
                }

                i++;
            }

            command = new BFCommand();

            return false;
        }

        private bool TryGetComment(BFCommand previousCommand, int startIndex, string code, out string result)
        {
            result = string.Empty;

            if (!previousCommand.IsBeginComment())
            {
                return false;
            }

            if (this.TryGetNextTargetCommand(code.Skip(startIndex).Join(), this._config.EndComment, out var index))
            {
                result = code.Substring(startIndex, index);
                return true;
            }

            result = code.Skip(startIndex).Join();
            return true;
        }

        private bool TryGetTrivia(BFCommand previousCommand, int startIndex, string code, out string result)
        {
            const string pattern = @"^\s+$";

            result = string.Empty;

            if (previousCommand.IsBeginComment())
            {
                return false;
            }

            if (this.TryGetNextCommand(code.Skip(startIndex).Join(), out var command, out var length))
            {
                result = code.Substring(startIndex, length);
            }
            else
            {
                result = code.Skip(startIndex).Join();
            }

            if (!result.IsMatch(pattern))
            {
                var innerIndex = result
                    .Select((x, i) => new { Index = i, Value = x })
                    .SkipWhile(x => x.Value.IsMatch(pattern))
                    .FirstOrDefault()
                    .Index;

                throw new BFSyntaxException(0, startIndex + innerIndex);
            }

            return true;
        }

        private BFCommand ReturnCommand(BFCommand command, ref int index, out BFCommand previousCommand)
        {
            index += command.Length;
            previousCommand = command;
            return command;
        }

        private IEnumerable<BFCommand> AnalyzeCodeByLine(string line)
        {
            var previousCommand = new BFCommand();

            for (int i = 0; i < line.Length;)
            {
                var c = line[i].ToString();

                var command = this._definedCommands
                    .Where(x => x.Command.StartsWith(c))
                    .Where(x => x.Length <= line.Length - i)
                    .FirstOrDefault(x => line.Substring(i, x.Length).Equals(x.Command));

                if (!previousCommand.IsBeginComment() && command.IsDefinedCommand())
                {
                    yield return this.ReturnCommand(command, ref i, out previousCommand);
                    continue;
                }

                if (this.TryGetTrivia(previousCommand, i, line, out var result))
                {
                    var trivia = new BFCommand(result, BFCommandType.Trivia);
                    yield return this.ReturnCommand(trivia, ref i, out previousCommand);
                    continue;
                }
                else if (this.TryGetComment(previousCommand, i, line, out result))
                {
                    var comment = new BFCommand(result, BFCommandType.Trivia);
                    yield return this.ReturnCommand(comment, ref i, out previousCommand);
                    continue;
                }
                else
                {
                    throw new BFSyntaxException(0, i);
                }
            }
        }

        private IEnumerable<BFCommand> AnalyzeCode(string code)
        {
            var splitedCode = code.SplitNewLine();

            var analized = new List<BFCommand>();

            for (int i = 0; i < splitedCode.Count(); i++)
            {
                var line = splitedCode.ElementAt(i);

                try
                {
                    analized.AddRange(this.AnalyzeCodeByLine(line));
                }
                catch (BFSyntaxException e)
                {
                    throw new BFSyntaxException(i, e.Index);
                }
                catch
                {
                    throw;
                }
            }

            return analized;
        }
    }
}
