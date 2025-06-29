namespace Entity
{
    public class ArtworkGallery
    {
        private int aArtworkId;
        private int aGalleryId;

        // default
        public ArtworkGallery() { }

        // parameterised
        public ArtworkGallery(int artworkId, int galleryId) { 
            this.aArtworkId = artworkId;
            this.aGalleryId = galleryId;
        }

        public int ArtworkID { get { return aArtworkId; } set { aArtworkId = value; } }

        public int GalleryID { get {return aGalleryId; } set {aGalleryId = value; } }   
    }
}
