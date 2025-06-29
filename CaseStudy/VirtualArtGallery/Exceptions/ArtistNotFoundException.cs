using System;

namespace Exceptions
{
    public class ArtistNotFoundException : Exception
    {
        public ArtistNotFoundException() : base("artist not found ") { }

        public ArtistNotFoundException(string message) : base($"artist not found: {message}") { }

        public ArtistNotFoundException(int ArtistID) : base($"artist with ID {ArtistID} not found") { }

        public ArtistNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
