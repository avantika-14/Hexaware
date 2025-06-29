using ContextEF;
using EntityEF;
using Exceptions;
using System;
using System.Data.Entity; 
using System.Data.Entity.Infrastructure;
using System.Linq;


namespace DAOEF
{
    public class VirtualArtGalleryImplementationEF : IVirtualArtGalleryEF
    {
        public bool AddGallery(Gallery Gallery)
        {
            using (var context = new ArtGalleryDBContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // 1. Validate curator exists
                        var artist = context.Artists.Find(Gallery.CuratorID);
                        if (artist == null)
                            throw new ArtistNotFoundException($"Artist/Curator with ID {Gallery.CuratorID} not found");

                        // 3. Add gallery
                        context.Gallery.Add(Gallery);
                        int affected = context.SaveChanges();

                        // 4. Set curator relationship
                        artist.CuratedGallery.Add(Gallery);
                        context.SaveChanges();

                        transaction.Commit();

                        Console.WriteLine($"Gallery '{Gallery.Name}' added successfully with ID: {Gallery.GalleryID}");
                        return true;
                    }
                    catch (ArtistNotFoundException ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"Validation Error: {ex.Message}");
                        throw;
                    }
                    catch (DbUpdateException ex)
                    {
                        transaction.Rollback();
                        // More detailed error message
                        Console.WriteLine($"Database Error: {(ex.InnerException?.InnerException?.Message ?? ex.InnerException?.Message ?? ex.Message)}");
                        return false;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"Unexpected Error: {ex.Message}");
                        return false;
                    }
                }
            }
        }

        public bool UpdateGallery(Gallery Gallery)
        {
            if (Gallery == null)
            {
                Console.WriteLine("Gallery object is null, cannot update");
                return false;
            }

            using (var context = new ArtGalleryDBContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // Get existing gallery with tracking
                        var existingGallery = context.Gallery
                            .FirstOrDefault(g => g.GalleryID == Gallery.GalleryID);

                        if (existingGallery == null)
                        {
                            Console.WriteLine($"Gallery with ID {Gallery.GalleryID} not found");
                            return false;
                        }

                        // Verify new curator exists
                        var artistExists = context.Artists
                            .Any(a => a.ArtistID == Gallery.CuratorID);

                        if (!artistExists)
                        {
                            throw new ArtistNotFoundException($"Artist with ID {Gallery.CuratorID} not found");
                        }

                        // Update only mutable properties
                        existingGallery.Name = Gallery.Name ?? existingGallery.Name;
                        existingGallery.Description = Gallery.Description ?? existingGallery.Description;
                        existingGallery.Location = Gallery.Location ?? existingGallery.Location;
                        existingGallery.OpeningHours = Gallery.OpeningHours ?? existingGallery.OpeningHours;
                        existingGallery.CuratorID = Gallery.CuratorID;

                        int affected = context.SaveChanges();
                        transaction.Commit();

                        Console.WriteLine(affected > 0
                            ? $"Gallery {Gallery.GalleryID} updated successfully"
                            : $"No changes made to Gallery {Gallery.GalleryID}");

                        return affected > 0;
                    }
                    catch (ArtistNotFoundException ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"Validation Error: {ex.Message}");
                        throw;
                    }
                    catch (GalleryNotFoundException ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"Gallery Error: {ex.Message}");
                        throw; // Re-throw to let calling code handle
                    }
                    catch (DbUpdateException ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"Database Update Failed: {ex.InnerException?.Message ?? ex.Message}");
                        Console.WriteLine(ex.ToString());
                        throw;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine($"Unexpected Error: {ex.Message}");
                        throw;
                    }
                }
            }
        }

        public Gallery GetGalleryByID(int GalleryID)
        {
            try
            {
                using (var context = new ArtGalleryDBContext())
                {
                    var gallery = context.Gallery
                                        .AsNoTracking()
                                        .Include(g => g.Curator)
                                        .Include(g => g.Artworks)
                                        .FirstOrDefault(g => g.GalleryID == GalleryID);

                    if (gallery == null)
                        throw new GalleryNotFoundException(GalleryID);

                    return gallery;
                }
            }
            catch (Exception ex)
            {
                // Log full error details
                Console.WriteLine($"Full error details: {ex.ToString()}");
                return null;
            }
        }
    }
}