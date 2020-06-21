using System;

namespace ImageFinder.CrossCutting.Exceptions
{
    public class ImageQueryException : Exception
    {
        public ImageQueryException()
        { }

        public ImageQueryException(Exception innerException)
            : base("Unable to perform search, Please refer inner exception for further details", innerException)
        { }
    }
}