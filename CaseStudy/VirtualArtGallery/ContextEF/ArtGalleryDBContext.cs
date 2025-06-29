using EntityEF;
using System.Data.Entity;
using Utils;

namespace ContextEF
{
    public class ArtGalleryDBContext : DbContext
    {
        public ArtGalleryDBContext() : base(DBProperty.GetConnectionString())
        {
        }

        public ArtGalleryDBContext(string connectionString) : base(connectionString)
        {
        }

        public DbSet<Artworks> Artworks { get; set; }

        public DbSet<Artists> Artists { get; set; }

        public DbSet<Users> Users { get; set; }

        public DbSet<Gallery> Gallery { get; set; }

        public DbSet<UserFavoriteArtwork> UserFavoriteArtworks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Configure the many-to-many relationship
            modelBuilder.Entity<Gallery>()
                .HasMany(g => g.Artworks)
                .WithMany(a => a.Gallery)
                .Map(m =>
                {
                    m.ToTable("artwork_gallery"); // Your actual table name
                    m.MapLeftKey("galleryID");    // Left column name
                    m.MapRightKey("artworkID");   // Right column name
                });

            // Disable initializer to prevent schema recreation
            Database.SetInitializer<ArtGalleryDBContext>(null);

            base.OnModelCreating(modelBuilder);
        }
    }
}

