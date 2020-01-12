using BFCore.Command;

namespace BFCore.Extesion
{
    public static class BFCommandTypeExtensions
    {
        public static bool IsCommandTypeOf(this BFCommandType commandType1, BFCommandType commandType2)
        {
            return commandType1.Equals(commandType2);
        }
    }
}
