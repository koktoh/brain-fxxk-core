using System.Runtime.Serialization;
using BFCore.Extesion;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BFCore.Command
{
    public struct BFCommand
    {
        public string Command { get; }
        [IgnoreDataMember]
        public int Length => this.Command.HasValue() ? this.Command.Length : 0;
        [JsonConverter(typeof(StringEnumConverter))]
        public BFCommandType CommandType { get; }
        [IgnoreDataMember]
        public int RowPos { get; }
        [IgnoreDataMember]
        public int Pos { get; }

        public BFCommand(string command, BFCommandType commandType, int rowPos, int pos)
        {
            this.Command = command;
            this.CommandType = commandType;
            this.RowPos = rowPos;
            this.Pos = pos;
        }

        [JsonConstructor]
        public BFCommand(string command, BFCommandType commandType) : this(command, commandType, 0, 0) { }

        public BFCommand(BFCommand baseBFCommand, int rowPos, int pos) : this(baseBFCommand.Command, baseBFCommand.CommandType, rowPos, pos) { }

        public override string ToString()
        {
            return $@"{{ {nameof(this.Command)}:""{this.Command}"", {nameof(this.Length)}:{this.Length}, {nameof(this.CommandType)}:{this.CommandType} }}";
        }
    }
}
