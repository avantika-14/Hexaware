
using DAO;
using DAO.Helpers;
using DAOEF;
using Exceptions;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using VirtualArtGallery.MainHelper;


namespace ArtGallery
{
    public class Program
    {

        static void Main(string[] args)
        {
            Console.Title = "Virtual Art Gallery";
            RunMainMenu();
        }

        static void RunMainMenu() {

            while (true)
            {
                ConsoleHelper.DisplayHeader("VIRTUAL ART GALLERY");

                Console.WriteLine("\n====== Welcome to Main Menu ======");
                Console.WriteLine("Click: ");
                Console.WriteLine("1 to Manage Artworks");
                Console.WriteLine("2 to Manage Favorites");
                Console.WriteLine("3 to Manage Galleries");
                Console.WriteLine("4 to Exit");

                Console.ForegroundColor= ConsoleColor.Yellow;
                Console.Write("\nSelect an option (1-4): ");

                string input = Console.ReadLine();
                SoundManager.PlayMenuSelection();

                switch (input)
                {
                    case "1":
                        RunArtworkMenu();
                        break;
                    case "2":
                        RunFavoritesMenu();
                        break;
                    case "3":
                        ManageGalleriesMenu();
                        break;
                    case "4":
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("\nThank you for Visiting! Exiting program...");
                        Console.ResetColor();
                        return;
                    default:
                        FeedbackManager.ShowError("Invalid Option!");
                        break;
                }
            }
        }

        static void RunArtworkMenu()
        {

            while (true)
            {
                ConsoleHelper.DisplayHeader("ARTWORK MANAGEMENT");

                Console.WriteLine("\n====== Welcome to Artwork Main Menu ======");
                Console.WriteLine("Click: ");
                Console.WriteLine("1 to Get Artwork by ID");
                Console.WriteLine("2 to Add New Artwork");
                Console.WriteLine("3 to Search Artwork");
                Console.WriteLine("4 to Update Artwork");
                Console.WriteLine("5 to Delete Artwork");
                Console.WriteLine("6 to Exit");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\nSelect an option (1-6): ");

                string input = Console.ReadLine();
                SoundManager.PlayMenuSelection();

                switch (input)
                {
                    case "1":
                        MainGetArtworkByID();
                        break;
                    case "2":
                        MainAddArtwork();
                        break;
                    case "3":
                        MainSearchArtworks();
                        break;
                    case "4":
                        MainUpdateArtwork();
                        break;
                    case "5":
                        MainDeleteArtwork();
                        break;
                    case "6":
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("\nThank you for Visiting! Exiting program...");
                        Console.ResetColor();
                        return;
                    default:
                        FeedbackManager.ShowError("Invalid Option!");
                        break;
                }
            }
        }

        static void RunFavoritesMenu()
        {
            while (true)
            {
                ConsoleHelper.DisplayHeader("FAVORITES MANAGEMENT");

                Console.WriteLine("\n====== Welcome to Favorites Main Menu ======");
                Console.WriteLine("Click: ");
                Console.WriteLine("1 to Get User Favorites");
                Console.WriteLine("2 to Add Artwork to Favorite");
                Console.WriteLine("3 to Remove Artwork from Favorite");
                Console.WriteLine("4 to Exit");

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\nSelect an option (1-4): ");

                string input = Console.ReadLine();
                SoundManager.PlayMenuSelection();

                switch (input)
                {
                    case "1":
                        MainGetUserFavorites();
                        break;
                    case "2":
                        MainAddToFavorite();
                        break;
                    case "3":
                        MainRemoveFromFavorites();
                        break;
                    case "4":
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("\nThank you for Visiting! Exiting program...");
                        Console.ResetColor();
                        return;
                    default:
                        FeedbackManager.ShowError("Invalid Option!");
                        break;
                }
            }
        }

        static void ManageGalleriesMenu()
        {
            var galleryService = new VirtualArtGalleryImplementationEF();

            ConsoleHelper.DisplayHeader("GALLERY MANAGEMENT");

            Console.WriteLine("\n====== Welcome to Gallery Main Menu ======");
            Console.WriteLine("Click: ");
            Console.WriteLine("1 to Add New Gallery");
            Console.WriteLine("2 to Update Gallery");
            Console.WriteLine("3 to View Gallery Details");
            Console.WriteLine("4 to Exit");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("\nSelect an option (1-4): ");

            string input = Console.ReadLine();
            SoundManager.PlayMenuSelection();

            switch (input)
            {
                case "1":
                    MainAddGallery(galleryService);
                    break;
                case "2":
                    MainUpdateGallery(galleryService);
                    break;
                case "3":
                    MainViewGallery(galleryService);
                    break;
                case "4":
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("\nThank you for Visiting! Exiting program...");
                    Console.ResetColor();
                    return;
                default:
                    FeedbackManager.ShowError("Invalid Option!");
                    break;
            }
        }
        static void MainAddArtwork()
        {
            ConsoleHelper.DisplayHeader("ADD ARTWORK TO GALLERY");

            try
            {
                var gallery = new VirtualArtGalleryImplementation();

                // Get user input
                Console.Write("Enter Title: ");
                string title = Console.ReadLine();

                Console.Write("Enter Description: ");
                string description = Console.ReadLine();

                Console.Write("Enter Medium (e.g., Oil on Canvas): ");
                string medium = Console.ReadLine();

                Console.Write("Enter Image URL: ");
                string imageUrl = Console.ReadLine();

                int artistId = ConsoleHelper.GetIntegerInput("Enter ArtistID: ");
                // Create new artwork
                var newArtwork = new Entity.Artworks(
                                            title,
                                            description,
                                            DateTime.Now, // Using current date for testing
                                            medium,
                                            imageUrl,
                                            artistId
                );

                // Add to database
                bool success = gallery.AddArtwork(newArtwork);

                if (success)
                {
                    FeedbackManager.ShowSuccess($"\nSuccessfully added Artwork with ID: {newArtwork.ArtworkID}");
                    FeedbackManager.ShowWarning("Pro tip: Use Option 1 to verify the new artwork exists!");
                }
                else
                {
                    FeedbackManager.ShowError("\nFailed to add Artwork");
                }
            }
            catch (ArtistNotFoundException ex)
            {
                FeedbackManager.ShowError(ex.Message);
            }
            catch (Exception ex)
            {
                FeedbackManager.ShowError(ex.Message);
            }

            ConsoleHelper.WaitForUser();
        }

        static void MainUpdateArtwork()
        {
            ConsoleHelper.DisplayHeader("UPDATE ARTWORK IN GALLERY");

            try
            {
                var gallery = new VirtualArtGalleryImplementation();

                // 1. Get artwork to update
                int ArtworkID = ConsoleHelper.GetIntegerInput("Enter ArtworkID: ");

                // 2. Fetch existing artwork
                Entity.Artworks existingArtwork = gallery.GetArtworkByID(ArtworkID);
                Console.WriteLine("\nCurrent Artwork Details:");
                ConsoleHelper.DisplayArtwork(existingArtwork);

                // 3. Get updated values
                Console.WriteLine("\nEnter new details (press Enter to keep current value):");

                Console.Write($"Title [{existingArtwork.Title}]: ");
                string title = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(title))
                    existingArtwork.Title = title;

                Console.Write($"Description [{existingArtwork.Description}]: ");
                string description = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(description))
                    existingArtwork.Description = description;

                Console.Write($"Medium [{existingArtwork.Medium}]: ");
                string medium = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(medium))
                    existingArtwork.Medium = medium;

                Console.Write($"Image URL [{existingArtwork.ImageURL}]: ");
                string imageUrl = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(imageUrl))
                    existingArtwork.ImageURL = imageUrl;

                Console.Write($"Artist ID [{existingArtwork.ArtistID}]: ");
                string artistIdInput = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(artistIdInput))
                {
                    if (int.TryParse(artistIdInput, out int artistId))
                        existingArtwork.ArtistID = artistId;
                }

                // 4. Perform update
                bool success = gallery.UpdateArtwork(existingArtwork);

                if (success)
                {
                    FeedbackManager.ShowSuccess("\nUpdate successful! Verifying changes...");

                    // Verify update
                    Entity.Artworks updatedArtwork = gallery.GetArtworkByID(ArtworkID);
                    Console.WriteLine("\nUpdated Artwork Details:");
                    ConsoleHelper.DisplayArtwork(updatedArtwork);
                }
            }
            catch (ArtworkNotFoundException ex)
            {
                FeedbackManager.ShowError(ex.Message);
            }
            catch (ArtistNotFoundException ex)
            {
                FeedbackManager.ShowError(ex.Message);
                if (ConsoleHelper.ConfirmAction("Return to main menu? (y/n): "))
                    return;
            }
            catch (Exception ex)
            {
                FeedbackManager.ShowError(ex.Message);
            }

            ConsoleHelper.WaitForUser();
        }

        static void MainDeleteArtwork()
        {
            ConsoleHelper.DisplayHeader("DELETE ARTWORK FROM GALLERY");

            try
            {
                var gallery = new VirtualArtGalleryImplementation();

                int ArtworkID = ConsoleHelper.GetIntegerInput("Enter ArtworkID: ");

                // Verify artwork exists first
                Entity.Artworks artworkToDelete = gallery.GetArtworkByID(ArtworkID);
                Console.WriteLine("\nArtwork Details:");
                ConsoleHelper.DisplayArtwork(artworkToDelete);

                // Check for references in other tables
                bool hasReferences = ConsoleHelper.CheckArtworkReferences(ArtworkID); // Call the method here

                if (hasReferences)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nWarning: This artwork is referenced in:");
                    Console.WriteLine("- User Favorites");
                    Console.WriteLine("- Gallery Exhibitions");
                    Console.ResetColor();

                    if (!ConsoleHelper.ConfirmAction("Are you sure you wish to delete ALL references and Artwork?"))
                    {
                        FeedbackManager.ShowWarning("\nDeletion cancelled.");
                        return; 
                    }
                }
                else
                {
                    if (!ConsoleHelper.ConfirmAction("Are you sure you wish to delete this Artwork?"))
                    {
                        FeedbackManager.ShowWarning("\nDeletion cancelled.");
                        return;
                    }
                }

                // Perform deletion
                bool success = gallery.RemoveArtwork(ArtworkID);

                if (success)
                {
                    FeedbackManager.ShowSuccess("\nArtwork deleted successfully!");

                    // Verification
                    try
                    {
                        gallery.GetArtworkByID(ArtworkID);
                        FeedbackManager.ShowWarning("Verification failed - Artwork still exists!");
                    }
                    catch (ArtworkNotFoundException)
                    {
                        FeedbackManager.ShowSuccess("Verification passed - Artwork no longer exists");
                    }
                }
                else
                {
                    FeedbackManager.ShowError("\nFailed to delete artwork");
                }
            }
            catch (ArtworkNotFoundException ex)
            {
                FeedbackManager.ShowError(ex.Message);
            }
            catch (Exception ex)
            {
                FeedbackManager.ShowError(ex.Message);
            }

            ConsoleHelper.WaitForUser();
        }



        static void MainGetArtworkByID()
        {
            ConsoleHelper.DisplayHeader("GET ARTWORK BY ID IN GALLERY");

            try
            {
                var gallery = new VirtualArtGalleryImplementation();

                int ID = ConsoleHelper.GetIntegerInput("\nEnter ID (0 to exit): ");

                if (ID == 0) // Add cancellation option
                {
                    FeedbackManager.ShowWarning("Operation cancelled.");
                    return;
                }

                Entity.Artworks artwork = gallery.GetArtworkByID(ID);
                ConsoleHelper.DisplayArtwork(artwork);  
            }

            catch (ArtworkNotFoundException ex)
            {
                FeedbackManager.ShowError(ex.Message);
            }
            catch (Exception ex)
            {
                FeedbackManager.ShowError(ex.Message);
            }

            ConsoleHelper.WaitForUser();
        }


        static void MainSearchArtworks()
        {
            ConsoleHelper.DisplayHeader("SEARCH ARTWORKS IN GALLERY");

            try
            {
                var gallery = new VirtualArtGalleryImplementation();

                string keyword = ConsoleHelper.GetStringInput("Enter search keyword (or leave blank to cancel): ");

                if (string.IsNullOrWhiteSpace(keyword))
                {
                    FeedbackManager.ShowWarning("Search cancelled.");
                    return;
                }

                List<Entity.Artworks> results = gallery.SearchArtworks(keyword);

                if (results.Count == 0)
                {
                    FeedbackManager.ShowWarning("No artworks found matching your search");
                }
                else
                {
                    FeedbackManager.ShowSuccess($"Found {results.Count} artwork(s): ");
                    Console.WriteLine(new string('-', 40));

                    foreach (var artwork in results)
                    {
                        ConsoleHelper.DisplayArtwork(artwork); // Reuse your existing display method
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
            catch (Exception ex)
            {
                FeedbackManager.ShowError($"Search error: {ex.Message}");
            }
            ConsoleHelper.WaitForUser();
        }


        // user favorite

        static void MainAddToFavorite()
        {
            ConsoleHelper.DisplayHeader("ADD ARTWORKS TO FAVORITES IN GALLERY");

            try
            {
                var gallery = new VirtualArtGalleryImplementation();

                int UserID = ConsoleHelper.GetIntegerInput("Enter UserID (0 to exit): ");
                if (UserID == 0)
                {
                    FeedbackManager.ShowWarning("Operation cancelled.");
                    return;
                }

                int ArtworkID = ConsoleHelper.GetIntegerInput("Enter ArtworkID: ");


                if (gallery.AddArtworkToFavorite(UserID, ArtworkID))
                {
                    FeedbackManager.ShowSuccess("Successfully added to favorites!");

                    // Optional: Show the added artwork
                    try
                    {
                        var artwork = gallery.GetArtworkByID(ArtworkID);
                        Console.WriteLine("\nAdded Artwork:");
                        ConsoleHelper.DisplayArtwork(artwork);
                    }
                    catch { /* Silent fail - optional feature */ }
                }
                else
                {
                    FeedbackManager.ShowError("Failed to add to favorites");
                }
            }
            catch (UserNotFoundException ex)
            {
                FeedbackManager.ShowError($"User error: {ex.Message}");
            }
            catch (ArtworkNotFoundException ex)
            {
                FeedbackManager.ShowError($"Artwork error: {ex.Message}");
            }
            catch (Exception ex)
            {
                FeedbackManager.ShowError($"Unexpected error: {ex.Message}");
            }
            finally
            {
                ConsoleHelper.WaitForUser();
            }
        }


        static void MainRemoveFromFavorites()
        {
            ConsoleHelper.DisplayHeader("REMOVE ARTWORK FROM FAVORITES IN GALLERY");

            try
            {
                var gallery = new VirtualArtGalleryImplementation();

                int UserID = ConsoleHelper.GetIntegerInput("Enter UserID (0 to cancel): ");

                if (UserID == 0) // Add cancellation option
                {
                    FeedbackManager.ShowWarning("Operation cancelled.");
                    return;
                }

                //first check if user has favorites
                if (!DBHelpers.CheckIfUserHasFavorites(UserID))
                {
                    FeedbackManager.ShowError("\nThis user has no favorite Artworks");
                    return;
                }

                // show user's favorites
                Console.WriteLine("\nUser's Favorite Artworks:");
                var favorites = gallery.GetUserFavoriteArtworks(UserID);
                foreach (var artwork in favorites)
                {
                    Console.WriteLine($"- ID: {artwork.ArtworkID}, Title: {artwork.Title}");
                }

                int ArtworkID = ConsoleHelper.GetIntegerInput("Enter ArtworkID to remove: ");


                if (!ConsoleHelper.ConfirmAction("Are you sure?")) // Use helper method
                {
                    FeedbackManager.ShowWarning("Operation cancelled.");
                    return;
                }

                if (gallery.RemoveArtworkFromFavorite(UserID, ArtworkID))
                    FeedbackManager.ShowSuccess("\nSuccessfully removed from favorites!");
                else
                    FeedbackManager.ShowError("\nFailed to remove from favorites");
            }

            catch (UserNotFoundException ex)
            {
                FeedbackManager.ShowError(ex.Message);
            }
            catch (Exception ex)
            {
                FeedbackManager.ShowError(ex.Message);
            }

            ConsoleHelper.WaitForUser();
        }

        static void MainGetUserFavorites()
        {
            ConsoleHelper.DisplayHeader("GET USER'S FAVORITE ARTWORK IN GALLERY");

            try
            {
                var gallery = new VirtualArtGalleryImplementation();

                int UserID = ConsoleHelper.GetIntegerInput("Enter UserID: ");

                // First check if user has favorites
                if (!DBHelpers.CheckIfUserHasFavorites(UserID))
                {
                    FeedbackManager.ShowWarning("\nThis user has no favorite artworks.");
                    ConsoleHelper.WaitForUser();
                    return;
                }

                // Get and display favorites
                var favorites = gallery.GetUserFavoriteArtworks(UserID);
                Console.WriteLine("\nUser's Favorite Artworks:");
                Console.WriteLine("--------------------------");

                foreach (var artwork in favorites)
                {
                    ConsoleHelper.DisplayArtwork(artwork);
                }
            }
            catch (UserNotFoundException ex)
            {
                FeedbackManager.ShowError(ex.Message);
            }
            catch (Exception ex)
            {
                FeedbackManager.ShowError(ex.Message);
            }

            ConsoleHelper.WaitForUser();
        }

  
        // gallery management
        static void MainAddGallery(VirtualArtGalleryImplementationEF service)
        {
            ConsoleHelper.DisplayHeader("ADD NEW GALLERY");

            try
            {
                var gallery = new EntityEF.Gallery();

                Console.Write("Enter Gallery Name: ");
                gallery.Name = Console.ReadLine();

                Console.Write("Enter Description: ");
                gallery.Description = Console.ReadLine();

                Console.Write("Enter Location: ");
                gallery.Location = Console.ReadLine();

                string input;
                Regex timeFormat = new Regex(@"^\d{2}:\d{2}\s*-\s*\d{2}:\d{2}$");

                while (true)
                {
                    Console.Write("Enter Opening Hours (format: 00:00 - 00:00): ");
                    input = Console.ReadLine();

                    if (timeFormat.IsMatch(input))
                    {
                        gallery.OpeningHours = input;
                        break;
                    }
                    else
                    {
                        FeedbackManager.ShowError("Invalid format! Please use format: HH:MM - HH:MM (e.g., 09:00 - 18:00)");
                    }
                }


                gallery.CuratorID = ConsoleHelper.GetIntegerInput("Enter Curator (Artist) ID: ");

                bool success = service.AddGallery(gallery);

                if (success)
                {
                    FeedbackManager.ShowSuccess($"Gallery '{gallery.Name}' added successfully!");
                }
                else
                {
                    FeedbackManager.ShowError("Failed to add gallery");
                }
            }
            catch (ArtistNotFoundException ex)
            {
                FeedbackManager.ShowError(ex.Message);
            }
            catch (Exception ex)
            {
                FeedbackManager.ShowError(ex.Message);
            }

            ConsoleHelper.WaitForUser();
        }

        static void MainUpdateGallery(VirtualArtGalleryImplementationEF service)
        {
            ConsoleHelper.DisplayHeader("UPDATE GALLERY");

            try
            {
                int galleryId = ConsoleHelper.GetIntegerInput("Enter Gallery ID to update: ");
                var gallery = service.GetGalleryByID(galleryId);

                if (gallery == null)  // Additional null check for safety
                {
                    FeedbackManager.ShowError("Gallery not found!");
                    return;
                }

                // Display current details
                Console.WriteLine("\nCurrent Gallery Details:");
                Console.WriteLine($"Name: {gallery.Name}");
                Console.WriteLine($"Description: {gallery.Description}");
                Console.WriteLine($"Location: {gallery.Location}");
                Console.WriteLine($"Curator ID: {gallery.CuratorID}");

                // Get updates
                Console.WriteLine("\nEnter new details (press Enter to keep current):");
                gallery.Name = ConsoleHelper.GetUpdatedValue("Name", gallery.Name);
                gallery.Description = ConsoleHelper.GetUpdatedValue("Description", gallery.Description);
                gallery.Location = ConsoleHelper.GetUpdatedValue("Location", gallery.Location);
                gallery.CuratorID = ConsoleHelper.GetUpdatedInt("Curator ID", gallery.CuratorID);

                // Save changes
                bool success = service.UpdateGallery(gallery);
                if (success)
                    FeedbackManager.ShowSuccess("Gallery updated successfully!");
                else
                    FeedbackManager.ShowError("Failed to update gallery");
            }
            catch (GalleryNotFoundException ex)  // Specific exception first
            {
                FeedbackManager.ShowError(ex.Message);

                if (ConsoleHelper.ConfirmAction("Would you like to try again with a different ID? (y/n): "))
                    MainUpdateGallery(service); // retry
            }
            catch (Exception ex)
            {
                FeedbackManager.ShowError($"Update failed: {ex.Message}");
                // For debugging:
                Console.WriteLine($"Full error details: {ex.ToString()}");
            }
            finally
            {
                ConsoleHelper.WaitForUser();
            }
        }       

        static void MainViewGallery(VirtualArtGalleryImplementationEF service)
        {
            ConsoleHelper.DisplayHeader("VIEW GALLERY DETAILS");

            try
            {
                int galleryId = ConsoleHelper.GetIntegerInput("Enter Gallery ID: ");
                var gallery = service.GetGalleryByID(galleryId);

                if (gallery != null)
                {
                    Console.WriteLine("\nGallery Details:");
                    Console.WriteLine($"ID: {gallery.GalleryID}");
                    Console.WriteLine($"Name: {gallery.Name}");
                    Console.WriteLine($"Location: {gallery.Location}");

                    Console.WriteLine($"\nCurator: {(gallery.Curator != null ? gallery.Curator.Name : "None assigned")}");

                    Console.WriteLine($"\nArtworks ({gallery.Artworks.Count}):");
                    foreach (var artwork in gallery.Artworks)
                    {
                        Console.WriteLine($"- {artwork.Title} (ID: {artwork.ArtworkID})");
                    }
                }

                ConsoleHelper.WaitForUser();
            }
            catch (GalleryNotFoundException ex)
            {
                FeedbackManager.ShowError(ex.Message);
                if (ConsoleHelper.ConfirmAction("Would you like to try a different ID? (y/n): "))
                {
                    MainViewGallery(service);
                }
            }
            catch (Exception ex)
            {
                FeedbackManager.ShowError($"Error: {ex.Message}");
            }

            ConsoleHelper.WaitForUser();
        }
    }
}
