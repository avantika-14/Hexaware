using System;

namespace Exceptions
{
    public class ArtworkNotFoundException : Exception
    {
        public ArtworkNotFoundException() : base("Artwork not found in the database") { }

        public ArtworkNotFoundException(string message) : base($"artwork not found: {message}") { }

        public ArtworkNotFoundException(int ArtworkID) : base($"artwork with ID {ArtworkID} does not exist") { }

        public ArtworkNotFoundException (string message, Exception innerException) : base(message, innerException) { }
    }
}
