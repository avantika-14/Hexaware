using System.Data.SqlClient;
using Utils;

namespace DAO.Helpers
{
    public static class Validators
    {
        public static bool ArtworkExists(int artworkID)
        {
            const string query = "SELECT COUNT(*) FROM artworks WHERE artworkID = @artworkID";

            using (SqlConnection conn = DBConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@artworkID", artworkID);
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public static bool ArtistExists(int artistID)
        {
            const string query = "SELECT COUNT(*) FROM artists WHERE artistID = @artistID";

            using (SqlConnection conn = DBConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@artistID", artistID);
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public static bool UserExists(int userID)
        {
            const string query = "SELECT COUNT(*) FROM users WHERE userID = @userID";

            using (SqlConnection conn = DBConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@userID", userID);
                return (int)cmd.ExecuteScalar() > 0;
            }
        }
    }
}
