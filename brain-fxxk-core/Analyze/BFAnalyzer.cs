using System.Collections.Generic;
using System.IO;
using System.Linq;
using BFCore.Command;
using BFCore.Config;
using BFCore.Exception;
using BFCore.Extesion;
using BFCore.Parse;

namespace BFCore.Analyze
{
    public class BFAnalyzer
    {
        private readonly BFCommandConfig _config;
        private readonly IReadOnlyList<BFCommand> _definedCommands;
        private readonly IParser _parser;

        public BFAnalyzer(CommonConfig commonConfig, BFCommandConfig commandConfig)
        {
            this._config = commandConfig;
            this._definedCommands = commandConfig.GetCommands().ToList();
            var parserFactory = new ParserFactory(commandConfig);
            this._parser = parserFactory.Create(commonConfig.EnableCommentOut);
        }

        public IEnumerable<BFCommand> Analyze(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                return this.Analyze(sr.ReadToEnd());
            }
        }

        public IEnumerable<BFCommand> Analyze(string code)
        {
            return this.AnalyzeCode(code);
        }

        private BFCommand GetSurplusLoopCommand(IEnumerable<BFCommand> commands)
        {
            return commands.Where(x => x.IsLoopHead() || x.IsLoopTail())
                .Aggregate<BFCommand, IDictionary<BFCommand, BFCommand>, BFCommand>(
                new Dictionary<BFCommand, BFCommand>(),
                (acc, next) =>
                {
                    if (next.IsBeginComment())
                    {
                        acc.Add(next, default);
                        return acc;
                    }
                    else
                    {
                        var key = acc.LastOrDefault(x => x.Value.Equals(default)).Key;
                        acc[key] = next;

                        return acc;
                    }
                },
                dict =>
                {
                    var result = dict.FirstOrDefault(x => x.Key.Equals(default) || x.Value.Equals(default));

                    return result.Key.Equals(default) ? result.Value : result.Key;
                });
        }

        private bool HasLoopCommandPair(IEnumerable<BFCommand> commands)
        {
            return commands.Where(x => x.IsLoopHead() || x.IsLoopTail())
                .Count() % 2 == 0;
        }

        private IEnumerable<BFCommand> AggregateComment(IEnumerable<BFCommand> commands)
        {
            var inComment = false;

            foreach(var commandsByLine in commands.GroupBy(x=>x.RowPos))
            {
                BFCommand trivia;

                foreach(var command in commandsByLine)
                {
                    if(command.IsBeginComment())
                    {
                        inComment = true;
                        yield return command;
                        continue;
                    }

                    if(inComment)
                    {
                        trivia = new BFCommand(command, command.RowPos, command.Pos);
                        continue;
                    }
                }
            }
        }

        private IEnumerable<BFCommand> AnalyzeCode(string code)
        {
            var parsed = this._parser.Parse(code);

            if (!this.HasLoopCommandPair(parsed))
            {
                var surplusLoopCommand = this.GetSurplusLoopCommand(parsed);

                throw new BFSyntaxException(surplusLoopCommand.RowPos, surplusLoopCommand.Pos, "Loop commands were not mutch.");
            }

            return parsed;
        }
    }
}
