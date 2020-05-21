namespace BFCore.Parse
{
    internal interface IParserFactory
    {
        IParser Create(bool enableCommentOut);
    }
}
