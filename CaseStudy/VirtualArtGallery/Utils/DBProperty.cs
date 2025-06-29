using System;
using System.Configuration;


namespace Utils
{
    public class DBProperty
    {
        //public static string GetConnectionString()
        //{
        //    return ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;
        //}
        public static string GetConnectionString()
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["MyConnection"]?.ConnectionString;

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new Exception("Connection string 'MyConnection' not found or empty in config file");
                }

                return connectionString;
            }
            catch (ConfigurationErrorsException ex)
            {
                throw new Exception("Failed to read configuration: " + ex.Message);
            }
        }
    }
}

