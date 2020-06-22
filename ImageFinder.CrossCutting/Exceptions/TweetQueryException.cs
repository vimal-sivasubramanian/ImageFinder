using System;

namespace ImageFinder.CrossCutting.Exceptions
{
    public class TweetQueryException : Exception
    {
        public TweetQueryException()
        { }

        public TweetQueryException(Exception innerException)
            : base("Unable to perform search, Please refer inner exception for further details", innerException)
        { }
    }
}
