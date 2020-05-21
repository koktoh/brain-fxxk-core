using System.Collections.Generic;
using System.IO;
using System.Linq;
using BFCore.Command;
using BFCore.Extesion;

namespace BFCore.Parse
{
    internal abstract class ParserBase : IParser
    {
        protected int _currentRowPos;

        public IEnumerable<BFCommand> Parse(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                return this.Parse(sr.ReadToEnd());
            }
        }

        public IEnumerable<BFCommand> Parse(string code)
        {
            return this.ParseCode(code);
        }

        protected abstract IEnumerable<BFCommand> ParseByRow(string row);

        protected IEnumerable<BFCommand> ParseCode(string code)
        {
            var rows = code.SplitNewLine().ToList();

            var parsed = new List<BFCommand>();

            for (this._currentRowPos = 0; this._currentRowPos < rows.Count; this._currentRowPos++)
            {
                var row = rows[this._currentRowPos];

                parsed.AddRange(this.ParseByRow(row));
            }

            return parsed;
        }
    }
}
