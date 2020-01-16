using System.Collections.Generic;
using BFCore.Command;

namespace BFCore.Config
{
    public class BFCommandConfig
    {
        public BFCommand Increment { get; set; } = new BFCommand("+", BFCommandType.Increment);
        public BFCommand Decrement { get; set; } = new BFCommand("-", BFCommandType.Decrement);
        public BFCommand MoveRight { get; set; } = new BFCommand(">", BFCommandType.MoveRight);
        public BFCommand MoveLeft { get; set; } = new BFCommand("<", BFCommandType.MoveLeft);
        public BFCommand RoopHead { get; set; } = new BFCommand("[", BFCommandType.RoopHead);
        public BFCommand RoopTale { get; set; } = new BFCommand("]", BFCommandType.RoopTale);
        public BFCommand Read { get; set; } = new BFCommand(",", BFCommandType.Read);
        public BFCommand Write { get; set; } = new BFCommand(".", BFCommandType.Write);
        public BFCommand BeginComment { get; set; } = new BFCommand("#", BFCommandType.BeginComment);
        public BFCommand EndComment { get; set; } = new BFCommand(";", BFCommandType.EndComment);

        public IEnumerable<BFCommand> GetCommands()
        {
            yield return this.Increment;
            yield return this.Decrement;
            yield return this.MoveRight;
            yield return this.MoveLeft;
            yield return this.RoopHead;
            yield return this.RoopTale;
            yield return this.Read;
            yield return this.Write;
            yield return this.BeginComment;
            yield return this.EndComment;
        }
    }
}
