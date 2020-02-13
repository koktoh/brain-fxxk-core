﻿using System.Runtime.Serialization;
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

        public BFCommand(string command, BFCommandType commandType)
        {
            this.Command = command;
            this.CommandType = commandType;
        }

        public override string ToString()
        {
            return $@"{{ {nameof(this.Command)}:""{this.Command}"", {nameof(this.Length)}:{this.Length}, {nameof(this.CommandType)}:{this.CommandType} }}";
        }
    }
}
