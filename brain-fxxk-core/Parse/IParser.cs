using System.Collections.Generic;
using System.IO;
using BFCore.Command;

namespace BFCore.Parse
{
    internal interface IParser
    {
        IEnumerable<BFCommand> Parse(Stream stream);
        IEnumerable<BFCommand> Parse(string code);
    }
}
