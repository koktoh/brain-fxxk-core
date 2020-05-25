using System.Text;
using BFCore.Command;
using BFCore.Extesion;

namespace BFCore.Exception
{
    public class BFRuntimeException : BFException
    {
        public BFCommand Command { get; } = default;

        public int LineNumber
        {
            get
            {
                if (this.Command.IsDefinedCommand())
                {
                    return this.Command.RowPos;
                }
                else
                {
                    return 0;
                }
            }
        }

        public int Index
        {
            get
            {
                if (this.Command.IsDefinedCommand())
                {
                    return this.Command.Pos;
                }
                else
                {
                    return 0;
                }
            }
        }

        public BFRuntimeException() : base() { }
        public BFRuntimeException(string message) : base(message) { }
        public BFRuntimeException(string message, System.Exception innerException) : base(message, innerException) { }
        public BFRuntimeException(BFCommand command) : base(BuildErrorMessage(command))
        {
            this.Command = command;
        }

        public BFRuntimeException(BFCommand command, System.Exception innerException) : base(BuildErrorMessage(command), innerException)
        {
            this.Command = command;
        }

        public BFRuntimeException(BFCommand command, string message) : base(BuildErrorMessage(command, message))
        {
            this.Command = command;
        }

        public BFRuntimeException(BFCommand command, string message, System.Exception innerException) : base(BuildErrorMessage(command, message), innerException)
        {
            this.Command = command;
        }

        private static string BuildErrorMessage(BFCommand command, string message = "")
        {
            var builder = new StringBuilder();

            builder.Append($"BFRuntimeError at line:{command.RowPos + 1} index:{command.Pos + 1}.");

            if (message.HasMeaningfulValue())
            {
                builder.AppendLine($" {message}");
            }
            else
            {
                builder.AppendLine();
            }

            builder.AppendLine(command.ToString());

            return builder.ToString();
        }
    }
}
