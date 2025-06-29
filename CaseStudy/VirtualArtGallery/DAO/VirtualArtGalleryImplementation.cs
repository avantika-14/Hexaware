using Entity;
using Exceptions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Utils;
using DAO.Helpers;

namespace DAO
{
    public class VirtualArtGalleryImplementation : IVirtualArtGallery
    {
        // artwork management

        public bool AddArtwork(Artworks Artwork)
        {
            // 1 validate artist exists
            if (!Validators.ArtistExists(Artwork.ArtistID))
                throw new ArtistNotFoundException(Artwork.ArtistID);

            try
            {
                // 2 SQL query
                const string query = @"INSERT INTO artworks 
                                (title, description, creationDate, medium, imageURL, artistID)
                                VALUES
                                (@title, @description, @creationDate, @medium, @imageURL, @artistID);
                                SELECT SCOPE_IDENTITY();"; // auto generated ID

                // 3 execute with parameterized query
                using (SqlConnection conn = DBConnection.GetConnection())
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    // Add parameters
                    cmd.Parameters.AddWithValue("@title", Artwork.Title);
                    cmd.Parameters.AddWithValue("@description", Artwork.Description ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@creationDate", Artwork.CreationDate);
                    cmd.Parameters.AddWithValue("@medium", Artwork.Medium);
                    cmd.Parameters.AddWithValue("@imageURL", Artwork.ImageURL);
                    cmd.Parameters.AddWithValue("@artistID", Artwork.ArtistID);

                    // 4. Execute and get new ID
                    Artwork.ArtworkID = Convert.ToInt32(cmd.ExecuteScalar());
                    return Artwork.ArtworkID > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error while adding Artwork: ", ex);
            }
        }

        public bool UpdateArtwork(Artworks Artwork)
        {
            // 1. check null
            if (Artwork == null)
            {
                Console.WriteLine("Artworks is null, cannot update");
                throw new ArgumentNullException(nameof(Artwork));
            }

            // 2 validate artwork exist
            if (!Validators.ArtworkExists(Artwork.ArtworkID))
            {
                Console.WriteLine($"ArtworkID {Artwork.ArtworkID} not found, it does not exist");
                throw new ArtworkNotFoundException(Artwork.ArtworkID);
            }

            // validate artist exists
            if (!Validators.ArtistExists(Artwork.ArtistID))
            {
                Console.WriteLine($"ArtistID {Artwork.ArtistID} not found");
                throw new ArtistNotFoundException(Artwork.ArtistID);
            }

            if (Artwork.CreationDate < new DateTime(1753, 1, 1))
                Artwork.CreationDate = new DateTime(1753, 1, 1);

            Console.WriteLine($"Attempting to Update ArtworkID {Artwork.ArtworkID}");

            const string query = @"UPDATE artworks SET 
                                title = @title, description = @description, creationDate = @creationDate, 
                                medium = @medium, imageURL = @imageURL, artistID = @artistID 
                                where artworkID = @artworkID;";

            using (SqlConnection conn = DBConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();

                cmd.Parameters.AddWithValue("@title", Artwork.Title);
                cmd.Parameters.AddWithValue("@description", Artwork.Description ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@creationDate", Artwork.CreationDate);
                cmd.Parameters.AddWithValue("@medium", Artwork.Medium);
                cmd.Parameters.AddWithValue("@imageURL", Artwork.ImageURL);
                cmd.Parameters.AddWithValue("@artistID", Artwork.ArtistID);
                cmd.Parameters.AddWithValue("@artworkID", Artwork.ArtworkID);

                int rowsAffected = cmd.ExecuteNonQuery();
                bool success = rowsAffected > 0;
                Console.WriteLine(success ? $"successfully Updated ArtworkID {Artwork.ArtworkID}"
                                          : "No rows were Updated");

                return success;

            }
        }

        public bool RemoveArtwork(int ArtworkID)
        {
            // 1 validate artwork exists
            if (!Validators.ArtworkExists(ArtworkID))
            {
                Console.WriteLine($"ArtworkID {ArtworkID} not found");
                throw new ArtworkNotFoundException();
            }

            using (SqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // delete from junction table first
                        DBHelpers.DeleteFromTable(conn, transaction, "user_favorite_artworks", "artworkID", ArtworkID);
                        DBHelpers.DeleteFromTable(conn, transaction, "artwork_gallery", "artworkID", ArtworkID);

                        // delete main artwork record
                        DBHelpers.DeleteFromTable(conn, transaction, "artworks", "artworkID", ArtworkID);

                        transaction.Commit();
                        return true;
                    }

                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"Error Deleting Artwork: {ex.Message}");
                        throw;
                    }
                }
            }
        }


        public Artworks GetArtworkByID(int ArtworkID)
        {
            const string query = "SELECT * FROM artworks WHERE artworkID = @ArtworkID";

            using (SqlConnection conn = DBConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@artworkID", ArtworkID);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return DBHelpers.CreateArtworkFromReader(reader);
                    }
                }
            }

            throw new ArtworkNotFoundException(ArtworkID);
        }

        public List<Artworks> SearchArtworks(string keywords)
        {
            if (string.IsNullOrWhiteSpace(keywords))
            {
                // 1 check if null
                Console.WriteLine("Search term was empty - returning empty results");
                return new List<Artworks>();
            }

            var artworks = new List<Artworks>();
            string trimmedKeyword = keywords.Trim();

            // 2 LOGGING the search term
            Console.WriteLine($"Searching for: '{trimmedKeyword}'");

            const string query = @"SELECT * FROM artworks WHERE
                                title LIKE @keywords OR
                                description LIKE @keywords OR
                                medium LIKE @keywords";

            using (SqlConnection conn = DBConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@keywords", $"%{trimmedKeyword}%");

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    int resultCount = 0;

                    while (reader.Read())
                    {
                        artworks.Add(DBHelpers.CreateArtworkFromReader(reader));
                        resultCount++;
                    }

                    Console.WriteLine($"found {resultCount} matching artworks");
                }
            }
            return artworks;
        }

        // user favorites 

        public bool AddArtworkToFavorite(int UserID, int ArtworkID)
        {
            if (!Validators.UserExists(UserID))
                throw new UserNotFoundException(UserID);

            if (!Validators.ArtworkExists(ArtworkID))
                throw new ArtworkNotFoundException(ArtworkID);

            try
            {
                const string query = @"INSERT INTO user_favorite_artworks (userID, artworkID) 
                                    VALUES (@userID, @artworkID)";

                using (SqlConnection conn = DBConnection.GetConnection())
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@userID", UserID);
                    cmd.Parameters.AddWithValue("@artworkID", ArtworkID);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new FavoriteOperationException("Database error while adding favorite ", ex);
            }
        }

        public bool RemoveArtworkFromFavorite(int UserID, int ArtworkID)
        {
            //check if user has favorites
            if (!DBHelpers.CheckIfUserHasFavorites(UserID))
                throw new FavoriteOperationException($"User {UserID} has no favorite artworks");

            // verify the artwork is actually user's favorite
            var favorites = GetUserFavoriteArtworks(UserID);
            // linq method - any()
            if (!favorites.Any(a => a.ArtworkID == ArtworkID))
                throw new FavoriteOperationException($"Artwork {ArtworkID} not found in user {UserID}'s favorites");

            try
            {
                const string query = @"DELETE FROM user_favorite_artworks WHERE
                                    userID = @userID AND artworkID = @artworkID";

                using (SqlConnection conn = DBConnection.GetConnection())
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@userID", UserID);
                    cmd.Parameters.AddWithValue("@artworkID", ArtworkID);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new FavoriteOperationException("Database error while removing favorites ", ex);
            }
        }

        public List<Artworks> GetUserFavoriteArtworks(int UserID)
        {
            if (!Validators.UserExists(UserID))
                throw new UserNotFoundException();

            if (!DBHelpers.CheckIfUserHasFavorites(UserID))
                return new List<Artworks>();

            try
            {
                const string query = @"SELECT a.* FROM artworks a 
                                    INNER JOIN user_favorite_artworks ufa ON
                                    a.artworkID = ufa.artworkID
                                    WHERE ufa.userID = @userID";
                var fav = new List<Artworks>();

                using (SqlConnection conn = DBConnection.GetConnection())
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@userID", UserID);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            fav.Add(DBHelpers.CreateArtworkFromReader(reader));
                        }
                    }
                }
                return fav;
            }
            catch (SqlException ex)
            {
                throw new FavoriteOperationException("Database error while retrieving favorites ", ex);
            }
        }
    }
}
