using Entity;
using Exceptions;
using System;
using System.Data.SqlClient;
using Utils;

namespace DAO.Helpers
{
    public static class DBHelpers
    {
        public static bool CheckIfUserHasFavorites(int UserID)
        {
            if (!Validators.UserExists(UserID))
                throw new UserNotFoundException();

            const string query = @"SELECT COUNT(*) FROM user_favorite_artworks
                                    WHERE userID = @UserID";

            using (SqlConnection conn = DBConnection.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@userID", UserID);
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public static void DeleteFromTable(SqlConnection conn, SqlTransaction transaction, string tableName, string columnName, int Id)
        {
            string query = $"DELETE FROM {tableName} WHERE {columnName} = @Id";
            using (SqlCommand cmd = new SqlCommand(query, conn, transaction))
            {
                cmd.Parameters.AddWithValue("@Id", Id);
                cmd.ExecuteNonQuery();
            }
        }

        public static Artworks CreateArtworkFromReader(SqlDataReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            return new Artworks(
                Convert.ToInt32(reader["artworkID"]),
                reader["title"].ToString(),
                reader["description"].ToString(),
                Convert.ToDateTime(reader["creationDate"]),
                reader["medium"].ToString(),
                reader["imageURL"].ToString(),
                Convert.ToInt32(reader["artistID"])
            );
        }
    }
}
