namespace BFCore.Exception
{
    public class BFSyntaxException : System.Exception
    {
        public int LineNumber { get; } = 0;
        public int Index { get; } = 0;

        public BFSyntaxException() : base() { }
        public BFSyntaxException(string message) : base(message) { }
        public BFSyntaxException(string message, System.Exception innerException) : base(message, innerException) { }

        public BFSyntaxException(int lineNumber, int index) : base(BuildErrorMessage(lineNumber, index))
        {
            this.LineNumber = lineNumber;
            this.Index = index;
        }


        public BFSyntaxException(int lineNumber, int index, System.Exception innerException) : base(BuildErrorMessage(lineNumber, index), innerException)
        {
            this.LineNumber = lineNumber;
            this.Index = index;
        }

        private static string BuildErrorMessage(int lineNumber, int index)
        {
            return $"BFSyntaxError at line:{lineNumber + 1} index:{index + 1}";
        }
    }
}
