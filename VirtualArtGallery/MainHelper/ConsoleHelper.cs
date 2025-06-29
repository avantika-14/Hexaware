using Entity;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace VirtualArtGallery.MainHelper
{
    public static class ConsoleHelper
    {
        public static void WaitForUser()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nPress any key to continue...");
            Console.ResetColor();
            Console.ReadKey();
            Console.WriteLine("\n");
        }

        public static bool ConfirmAction(string message)
        {
            Console.ForegroundColor= ConsoleColor.Yellow;
            Console.WriteLine($"\n{message} (y/n): ");
            Console.ResetColor();
            SoundManager.PlayWarning();

            string confirm = Console.ReadLine()?.Trim().ToLower();
            return (confirm == "y" || confirm == "yes"); 
        }

        public static int GetIntegerInput(string prompt)
        {
            Console.WriteLine(prompt);

            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int result))
                    return result;
                FeedbackManager.ShowError("Invalid input. Please ensure a number.");
                Console.WriteLine(prompt);
            }
        }

        public static string GetStringInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine()?.Trim();
        }

        public static void DisplayArtwork(Artworks artwork)
        {
            Console.WriteLine("-------------------------------------");
            Console.WriteLine($"ID: {artwork.ArtworkID}");
            Console.WriteLine($"Title: {artwork.Title}");
            Console.WriteLine($"Description: {artwork.Description}");
            Console.WriteLine($"Medium: {artwork.Medium}");
            Console.WriteLine($"Image URL: {artwork.ImageURL}");
            Console.WriteLine($"Artist ID: {artwork.ArtistID}");
            Console.WriteLine($"Created: {artwork.CreationDate:yyyy-MM-dd}");
            Console.WriteLine("-------------------------------------");
        }

        public static void DisplayHeader(string title)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            string border = new string('═', title.Length + 6);
            Console.WriteLine($"╔{border}╗");
            Console.WriteLine($"║   {title.ToUpper()}   ║");
            Console.WriteLine($"╚{border}╝");
            Console.ResetColor();
        }

        public static bool CheckArtworkReferences(int ArtworkID)
        {
            const string checkFavoritesQuery = "select count(*) from user_favorite_artworks where artworkID = @artworkID";
            const string checkGalleryQuery = "select count(*) from artwork_gallery where artworkID = @artworkID";

            using (SqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();

                // Check user favorites
                using (SqlCommand cmd = new SqlCommand(checkFavoritesQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@artworkID", ArtworkID);
                    object result = cmd.ExecuteScalar();
                    if (result != null && Convert.ToInt32(result) > 0)
                        return true;

                }

                // Check gallery exhibitions
                using (SqlCommand cmd = new SqlCommand(checkGalleryQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@artworkID", ArtworkID);
                    object result = cmd.ExecuteScalar();
                    if (result != null && Convert.ToInt32(result) > 0)
                        return true;
                }
            }
            return false;
        }

        public static string GetUpdatedValue(string field, string current)
        {
            Console.Write($"{field} [{current}]: ");
            string input = Console.ReadLine();
            return string.IsNullOrWhiteSpace(input) ? current : input;
        }

        public static int GetUpdatedInt(string field, int current)
        {
            Console.Write($"{field} [{current}]: ");
            string input = Console.ReadLine();
            return string.IsNullOrWhiteSpace(input) ? current : int.Parse(input);
        }
    }
}
