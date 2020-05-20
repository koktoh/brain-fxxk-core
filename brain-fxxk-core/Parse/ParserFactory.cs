using BFCore.Config;

namespace BFCore.Parse
{
    internal class ParserFactory : IParserFactory
    {
        private readonly BFCommandConfig _commandConfig;

        public ParserFactory(BFCommandConfig commandConfig)
        {
            this._commandConfig = commandConfig;
        }

        public IParser Create(bool enableCommentOut)
        {
            if(enableCommentOut)
            {
                return new BFDefaultParser(this._commandConfig);
            }
            else
            {
                return new BFIgnoreCommentOutParser(this._commandConfig);
            }
        }
    }
}
