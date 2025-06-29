using System;
using System.Data.SqlClient;


namespace Utils
{
    public class DBConnection
    {
        public static SqlConnection GetConnection()
        {
            try
            {
                SqlConnection connection = new SqlConnection(DBProperty.GetConnectionString());

                if (string.IsNullOrEmpty(connection.ConnectionString))
                    throw new Exception("connection string not found in the config file");

                return connection;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting connection: {ex.Message}");
                throw;
            }
        }
    }
}

