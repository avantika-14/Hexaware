using System.Collections.Generic;
using Entity;

namespace DAO
{
    public interface IVirtualArtGallery
    {
        // artwork management
        bool AddArtwork(Artworks Artwork);

        bool UpdateArtwork(Artworks Artwork);

        bool RemoveArtwork(int ArtworkID);

        Artworks GetArtworkByID(int ArtworkID);

        List<Artworks> SearchArtworks(string keywords);

        //user favorites
        bool AddArtworkToFavorite(int UserID, int ArtworkID);

        bool RemoveArtworkFromFavorite(int UserID, int ArtworkID);

        List<Artworks> GetUserFavoriteArtworks(int UserID);
    } 
}