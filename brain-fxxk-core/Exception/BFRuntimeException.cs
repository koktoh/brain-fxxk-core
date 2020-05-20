namespace BFCore.Exception
{
    public class BFRuntimeException : BFException
    {
        public BFRuntimeException() : base() { }
        public BFRuntimeException(string message) : base(message) { }
        public BFRuntimeException(string message, System.Exception innerException) : base(message, innerException) { }
    }
}
