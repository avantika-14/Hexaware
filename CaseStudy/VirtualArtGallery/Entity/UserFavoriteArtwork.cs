namespace Entity
{
    public class UserFavoriteArtwork
    {
        private int fUserId;
        private int fArtworkId;

        // default
        public UserFavoriteArtwork() { }

        // parameterised
        public UserFavoriteArtwork (int userId, int artworkId)
        {
            this.fUserId = userId;
            this.fArtworkId = artworkId;
        }

        public int UserID { get { return fUserId; } set { fUserId = value; } }

        public int ArtworkID { get { return fArtworkId; } set { fArtworkId = value; } }
    }
}
