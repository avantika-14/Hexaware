using ContextEF;
using DAOEF;
using EntityEF;
using Exceptions;
using NUnit.Framework;
using System;
using System.Linq;

namespace ArtGalleryTesting
{
    [TestFixture]
    public class GalleryTestsEF
    {
        private IVirtualArtGalleryEF galleryService;
        private ArtGalleryDBContext context;

        [SetUp]
        public void Setup()
        {
            galleryService = new VirtualArtGalleryImplementationEF();
            context = new ArtGalleryDBContext();

            // Ensure we have at least one artist for curator relationships
            if (!context.Artists.Any())
            {
                context.Artists.Add(new Artists
                {
                    Name = "Test Curator",
                    Biography = "Test bio"
                });
                context.SaveChanges();
            }
        }

        [TearDown]
        public void Cleanup()
        {
            // Remove only test galleries we created
            var testGalleries = context.Gallery
                .Where(g => g.Name.StartsWith("Test_"))
                .ToList();

            context.Gallery.RemoveRange(testGalleries);
            context.SaveChanges();
            context.Dispose();
        }

        [Test]
        public void AddGallery_WithValidData_ReturnsTrue()
        {
            // Arrange
            var curator = context.Artists.First();
            var gallery = new Gallery
            {
                Name = "Test_Gallery_" + Guid.NewGuid(),
                Description = "Test Description",
                Location = "Test Location",
                OpeningHours = "9-5",
                CuratorID = curator.ArtistID
            };

            // Act
            bool result = galleryService.AddGallery(gallery);

            // Assert
            Assert.IsTrue(result);
            var dbGallery = context.Gallery.Find(gallery.GalleryID);
            Assert.IsNotNull(dbGallery);
            Assert.AreEqual(gallery.Name, dbGallery.Name);
        }

        [Test]
        public void AddGallery_WithInvalidCurator_ThrowsArtistNotFoundException()
        {
            // Arrange
            var gallery = new Gallery
            {
                Name = "Test_Gallery_Invalid",
                CuratorID = 99999 // Non-existent artist
            };

            // Act & Assert
            var ex = Assert.Throws<ArtistNotFoundException>(() => galleryService.AddGallery(gallery));
            Assert.IsNotNull(ex);
        }

        [Test]
        public void UpdateGallery_ValidUpdate_ReturnsTrue()
        {
            // Arrange
            var curator = context.Artists.First();
            var gallery = new Gallery
            {
                Name = "Test_Gallery_ToUpdate",
                Description = "Original Description",
                CuratorID = curator.ArtistID
            };
            context.Gallery.Add(gallery);
            context.SaveChanges();

            var updatedGallery = new Gallery
            {
                GalleryID = gallery.GalleryID,
                Name = "Test_Gallery_Updated",
                Description = "Updated Description",
                CuratorID = curator.ArtistID
            };

            // Act
            bool result = galleryService.UpdateGallery(updatedGallery);

            // Assert
            Assert.IsTrue(result);
            var dbGallery = context.Gallery.Find(gallery.GalleryID);
            context.Entry(dbGallery).Reload();
            Assert.AreEqual("Test_Gallery_Updated", dbGallery.Name);
            Assert.AreEqual("Updated Description", dbGallery.Description);
        }

        [Test]
        public void UpdateGallery_NonExistentGallery_ReturnsFalse()
        {
            // Arrange
            var gallery = new Gallery
            {
                GalleryID = 99999, // Non-existent gallery
                Name = "Test_NonExistent"
            };

            // Act
            bool result = galleryService.UpdateGallery(gallery);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void GetGalleryByID_ExistingGallery_ReturnsGallery()
        {
            // Arrange
            var curator = context.Artists.First();
            var gallery = new Gallery
            {
                Name = "Test_Gallery_ToRetrieve",
                CuratorID = curator.ArtistID
            };
            context.Gallery.Add(gallery);
            context.SaveChanges();

            // Act
            var result = galleryService.GetGalleryByID(gallery.GalleryID);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(gallery.GalleryID, result.GalleryID);
            Assert.AreEqual(gallery.Name, result.Name);
            Assert.AreEqual(curator.ArtistID, result.CuratorID);
        }

        [Test]
        public void GetGalleryByID_NonExistentGallery_ReturnsNull()
        {
            // Act
            var result = galleryService.GetGalleryByID(99999);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void AddGallery_NullGallery_ReturnsFalse()
        {
            // Act
            bool result = galleryService.AddGallery(null);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void UpdateGallery_NullGallery_ReturnsFalse()
        {
            // Act
            bool result = galleryService.UpdateGallery(null);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void UpdateGallery_ChangeCuratorToInvalidArtist_ThrowsArtistNotFoundException()
        {
            // Arrange
            var curator = context.Artists.First();
            var gallery = new Gallery
            {
                Name = "Test_Gallery_CuratorChange",
                CuratorID = curator.ArtistID
            };
            context.Gallery.Add(gallery);
            context.SaveChanges();

            var updatedGallery = new Gallery
            {
                GalleryID = gallery.GalleryID,
                CuratorID = 99999 // Invalid artist
            };

            // Act & Assert
            Assert.Throws<ArtistNotFoundException>(() => galleryService.UpdateGallery(updatedGallery));
        }
    }
}