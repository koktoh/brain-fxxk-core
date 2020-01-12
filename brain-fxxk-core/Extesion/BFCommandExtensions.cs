using System;
using System.Linq;
using BFCore.Command;

namespace BFCore.Extesion
{
    public static class BFCommandExtensions
    {
        public static bool IsUndefined(this BFCommand command)
        {
            return command.CommandType.IsCommandTypeOf(BFCommandType.Undefined);
        }

        public static bool IsTrivia(this BFCommand command)
        {
            return command.CommandType.IsCommandTypeOf(BFCommandType.Trivia);
        }

        public static bool IsIncrement(this BFCommand command)
        {
            return command.CommandType.IsCommandTypeOf(BFCommandType.Increment);
        }

        public static bool IsDecrement(this BFCommand command)
        {
            return command.CommandType.IsCommandTypeOf(BFCommandType.Decrement);
        }

        public static bool IsMoveRight(this BFCommand command)
        {
            return command.CommandType.IsCommandTypeOf(BFCommandType.MoveRight);
        }

        public static bool IsMoveLeft(this BFCommand command)
        {
            return command.CommandType.IsCommandTypeOf(BFCommandType.MoveLeft);
        }

        public static bool IsRoopHead(this BFCommand command)
        {
            return command.CommandType.IsCommandTypeOf(BFCommandType.RoopHead);
        }

        public static bool IsRoopTale(this BFCommand command)
        {
            return command.CommandType.IsCommandTypeOf(BFCommandType.RoopTale);
        }

        public static bool IsRead(this BFCommand command)
        {
            return command.CommandType.IsCommandTypeOf(BFCommandType.Read);
        }

        public static bool IsWrite(this BFCommand command)
        {
            return command.CommandType.IsCommandTypeOf(BFCommandType.Write);
        }

        public static bool IsBeginComment(this BFCommand command)
        {
            return command.CommandType.IsCommandTypeOf(BFCommandType.BeginComment);
        }

        public static bool IsEndComment(this BFCommand command)
        {
            return command.CommandType.IsCommandTypeOf(BFCommandType.EndComment);
        }

        public static bool IsExecutable(this BFCommand command)
        {
            return !(command.IsUndefined() || command.IsTrivia() || command.IsBeginComment() || command.IsEndComment());
        }

        public static bool IsDefinedCommand(this BFCommand command)
        {
            var definedTypes = ((BFCommandType[])Enum.GetValues(typeof(BFCommandType)))
                .Where(x => !x.IsCommandTypeOf(BFCommandType.Undefined));

            return definedTypes.Contains(command.CommandType);
        }
    }
}
