using System.Collections.Generic;
using System.Runtime.Serialization;
using BFCore.Command;
using BFCore.Extesion;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BFCore.Config
{
    public class BFCommandConfig
    {
        public BFCommand Increment { get; set; } = new BFCommand("+", BFCommandType.Increment);
        public BFCommand Decrement { get; set; } = new BFCommand("-", BFCommandType.Decrement);
        public BFCommand MoveRight { get; set; } = new BFCommand(">", BFCommandType.MoveRight);
        public BFCommand MoveLeft { get; set; } = new BFCommand("<", BFCommandType.MoveLeft);
        public BFCommand LoopHead { get; set; } = new BFCommand("[", BFCommandType.LoopHead);
        public BFCommand LoopTail { get; set; } = new BFCommand("]", BFCommandType.LoopTail);
        public BFCommand Read { get; set; } = new BFCommand(",", BFCommandType.Read);
        public BFCommand Write { get; set; } = new BFCommand(".", BFCommandType.Write);
        public BFCommand BeginComment { get; set; } = new BFCommand("#", BFCommandType.BeginComment);
        public BFCommand EndComment { get; set; } = new BFCommand(";", BFCommandType.EndComment);

        [JsonExtensionData]
        private readonly IDictionary<string, JToken> _additionalData;

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            const string roopHeadJsonKey = "RoopHead";
            const string roopTaleJsonKey = "RoopTale";

            if (this._additionalData.ContainsKey(roopHeadJsonKey))
            {
                var roopHeadCommand = (string)this._additionalData[roopHeadJsonKey][nameof(BFCommand.Command)];

                if (roopHeadCommand.HasMeaningfulValue())
                {
                    this.LoopHead = new BFCommand(roopHeadCommand, BFCommandType.LoopHead);
                }
            }

            if (this._additionalData.ContainsKey(roopTaleJsonKey))
            {
                var roopTaleCommand = (string)this._additionalData[roopTaleJsonKey][nameof(BFCommand.Command)];


                if (roopTaleCommand.HasMeaningfulValue())
                {
                    this.LoopTail = new BFCommand(roopTaleCommand, BFCommandType.LoopTail);
                }
            }

            this._additionalData.Clear();
        }

        public BFCommandConfig()
        {
            this._additionalData = new Dictionary<string, JToken>();
        }

        public IEnumerable<BFCommand> GetCommands()
        {
            yield return this.Increment;
            yield return this.Decrement;
            yield return this.MoveRight;
            yield return this.MoveLeft;
            yield return this.LoopHead;
            yield return this.LoopTail;
            yield return this.Read;
            yield return this.Write;
            yield return this.BeginComment;
            yield return this.EndComment;
        }
    }
}
