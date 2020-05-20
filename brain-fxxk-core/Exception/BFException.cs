namespace BFCore.Exception
{
    public class BFException : System.Exception
    {
        public BFException() : base() { }
        public BFException(string message) : base(message) { }
        public BFException(string message, System.Exception innerException) : base(message, innerException) { }
    }
}
