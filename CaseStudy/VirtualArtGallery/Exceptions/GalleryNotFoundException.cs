using System;

namespace Exceptions
{
    public class GalleryNotFoundException : Exception
    {
        public GalleryNotFoundException() : base("Gallery not found") { }
        
        public GalleryNotFoundException(string message) : base(message) { }
        
        public GalleryNotFoundException(string message, Exception inner) : base(message, inner) { }

        public GalleryNotFoundException(int GalleryID) : base($"Gallery with ID {GalleryID} does not exist") { }
    }
}
