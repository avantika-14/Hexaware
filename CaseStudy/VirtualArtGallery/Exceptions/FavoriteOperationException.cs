using System;


namespace Exceptions
{
    public class FavoriteOperationException : Exception
    {

        public FavoriteOperationException() : base("Favorite operation failed") { }

        public FavoriteOperationException(string message) : base($"Favorite operation failed: {message}") { }

        public FavoriteOperationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
