using DAO;
using Entity;
using Exceptions;
using NUnit.Framework;
using System;
using System.Data.SqlClient;
using System.Linq;
using Utils;

namespace ArtGalleryTesting
{
    [TestFixture]
    public class ArtworkAndFavoritesTestADO
    {
        private IVirtualArtGallery galleryService;
        private int testArtistId;
        private int testArtworkId;
        private int testUserId;
        private SqlConnection testConnection;

        [SetUp]
        public void Setup()
        {
            galleryService = new VirtualArtGalleryImplementation();
            testConnection = DBConnection.GetConnection();
            testConnection.Open();

            // Create fresh test data for each test
            testArtistId = CreateTestArtist();
            testArtworkId = CreateTestArtwork(testArtistId);
            testUserId = CreateTestUser();
        }

        [TearDown]
        public void Cleanup()
        {
            // Clean up all test data
            RemoveTestData(testArtworkId, testArtistId, testUserId);
            testConnection.Close();
        }

        #region Helper Methods
        private int CreateTestArtist()
        {
            using (var cmd = new SqlCommand(
                @"INSERT INTO Artists (Name, Biography, BirthDate, Nationality, Website, ContactInformation)
                  VALUES ('Test Artist', 'Test Bio', @date, 'Test', 'http://test.com', 'test@test.com');
                  SELECT SCOPE_IDENTITY();", testConnection))
            {
                cmd.Parameters.AddWithValue("@date", DateTime.Now.Date);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private int CreateTestArtwork(int artistId)
        {
            using (var cmd = new SqlCommand(
                @"INSERT INTO Artworks (Title, Description, CreationDate, Medium, ImageURL, ArtistID)
                  VALUES ('Test Artwork', 'Test Desc', @date, 'Oil on canvas', 'http://test.com/img.jpg', @artistId);
                  SELECT SCOPE_IDENTITY();", testConnection))
            {
                cmd.Parameters.AddWithValue("@date", DateTime.Now.Date);
                cmd.Parameters.AddWithValue("@artistId", artistId);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private int CreateTestUser()
        {
            string uniqueId = Guid.NewGuid().ToString().Substring(0, 8);
            using (var cmd = new SqlCommand(
                @"INSERT INTO Users (Username, Password, Email, FirstName, LastName, DateOfBirth)
                  VALUES (@user, 'testpass', @email, 'Test', 'User', @dob);
                  SELECT SCOPE_IDENTITY();", testConnection))
            {
                cmd.Parameters.AddWithValue("@user", $"testuser_{uniqueId}");
                cmd.Parameters.AddWithValue("@email", $"{uniqueId}@test.com");
                cmd.Parameters.AddWithValue("@dob", DateTime.Now.Date.AddYears(-20));
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        private void RemoveTestData(int artworkId, int artistId, int userId)
        {
            try
            {
                // Remove favorites first
                new SqlCommand(
                    $"DELETE FROM user_favorite_artworks WHERE userID = {userId} OR artworkID = {artworkId}",
                    testConnection).ExecuteNonQuery();

                // Then remove artwork
                new SqlCommand(
                    $"DELETE FROM artworks WHERE artworkID = {artworkId}",
                    testConnection).ExecuteNonQuery();

                // Then remove artist
                new SqlCommand(
                    $"DELETE FROM artists WHERE artistID = {artistId}",
                    testConnection).ExecuteNonQuery();

                // Finally remove user
                new SqlCommand(
                    $"DELETE FROM users WHERE userID = {userId}",
                    testConnection).ExecuteNonQuery();
            }
            catch { /* Ignore cleanup errors */ }
        }
        #endregion

        #region Artwork Tests
        [Test]
        public void AddArtwork_ValidData_ReturnsTrue()
        {
            // Arrange
            var artwork = new Artworks
            {
                Title = "Test Artwork",
                Description = "Test Description",
                CreationDate = DateTime.Now.Date,
                Medium = "Oil on canvas",
                ImageURL = "http://test.com/image.jpg",
                ArtistID = testArtistId
            };

            // Act
            bool result = galleryService.AddArtwork(artwork);

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(artwork.ArtworkID > 0);
        }

        [Test]
        public void AddArtwork_InvalidArtist_ThrowsArtistNotFoundException()
        {
            // Arrange
            var artwork = new Artworks
            {
                Title = "Test Artwork",
                ArtistID = 99999 // Invalid artist
            };

            // Act & Assert
            Assert.Throws<ArtistNotFoundException>(() => galleryService.AddArtwork(artwork));
        }

        [Test]
        public void UpdateArtwork_ValidData_ReturnsTrue()
        {
            // Arrange
            var artwork = new Artworks
            {
                ArtworkID = testArtworkId,
                Title = "Updated Title",
                Description = "Updated Description",
                CreationDate = DateTime.Now.Date,
                Medium = "Updated Medium",
                ImageURL = "http://updated.com/image.jpg",
                ArtistID = testArtistId
            };

            // Act
            bool result = galleryService.UpdateArtwork(artwork);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void UpdateArtwork_NonExistentArtwork_ThrowsArtworkNotFoundException()
        {
            // Arrange
            var artwork = new Artworks
            {
                ArtworkID = 99999, // Invalid artwork
                Title = "Test",
                ArtistID = testArtistId
            };

            // Act & Assert
            Assert.Throws<ArtworkNotFoundException>(() => galleryService.UpdateArtwork(artwork));
        }

        [Test]
        public void RemoveArtwork_ExistingArtwork_ReturnsTrue()
        {
            // Arrange
            int testArtworkId = CreateTestArtwork(testArtistId); // Create separate artwork for this test

            // Act
            bool result = galleryService.RemoveArtwork(testArtworkId);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void GetArtworkByID_ExistingArtwork_ReturnsArtwork()
        {
            // Act
            var artwork = galleryService.GetArtworkByID(testArtworkId);

            // Assert
            Assert.IsNotNull(artwork);
            Assert.AreEqual(testArtworkId, artwork.ArtworkID);
        }

        [Test]
        public void SearchArtworks_ValidKeyword_ReturnsMatchingArtworks()
        {
            // Arrange
            string keyword = "Mona"; // From your seeded data

            // Act
            var results = galleryService.SearchArtworks(keyword);

            // Assert
            Assert.IsNotEmpty(results);
            Assert.IsTrue(results.Any(a => a.Title.Contains(keyword)));
        }

        [Test]
        public void SearchArtworks_EmptyKeyword_ReturnsEmptyList()
        {
            // Act
            var results = galleryService.SearchArtworks("");

            // Assert
            Assert.IsEmpty(results);
        }
        #endregion

        #region Favorite Tests
        [Test]
        public void AddArtworkToFavorite_ValidData_ReturnsTrue()
        {
            // Act
            bool result = galleryService.AddArtworkToFavorite(testUserId, testArtworkId);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void AddArtworkToFavorite_InvalidUser_ThrowsUserNotFoundException()
        {
            // Arrange
            int invalidUserId = 99999;

            // Act & Assert
            Assert.Throws<UserNotFoundException>(() =>
                galleryService.AddArtworkToFavorite(invalidUserId, testArtworkId));
        }

        [Test]
        public void RemoveArtworkFromFavorite_ExistingFavorite_ReturnsTrue()
        {
            // Arrange - Create isolated test data
            int userId = CreateTestUser();
            int artworkId = CreateTestArtwork(testArtistId);

            // Add exactly one favorite
            galleryService.AddArtworkToFavorite(userId, artworkId);

            // Act
            bool result = galleryService.RemoveArtworkFromFavorite(userId, artworkId);

            // Assert
            Assert.IsTrue(result, "Remove operation should return true");

            // Verify the favorite was actually removed
            var favorites = galleryService.GetUserFavoriteArtworks(userId);
            Assert.IsEmpty(favorites, "Favorites list should be empty after removal");
        }

        [Test]
        public void RemoveArtworkFromFavorite_NonExistentFavorite_ThrowsException()
        {
            // Arrange - Create test data but don't add to favorites
            int userId = CreateTestUser();
            int artworkId = CreateTestArtwork(testArtistId);

            // Act & Assert
            Assert.Throws<FavoriteOperationException>(() =>
                galleryService.RemoveArtworkFromFavorite(userId, artworkId));
        }

        [Test]
        public void GetUserFavoriteArtworks_UserWithFavorites_ReturnsCorrectArtworks()
        {
            // Arrange
            int userId = CreateTestUser();
            int artworkId1 = CreateTestArtwork(testArtistId);
            int artworkId2 = CreateTestArtwork(testArtistId);

            galleryService.AddArtworkToFavorite(userId, artworkId1);
            galleryService.AddArtworkToFavorite(userId, artworkId2);

            // Act
            var favorites = galleryService.GetUserFavoriteArtworks(userId);

            // Assert
            Assert.AreEqual(2, favorites.Count, "Should return exactly 2 favorites");
            Assert.IsTrue(favorites.Any(a => a.ArtworkID == artworkId1), "Missing first favorite");
            Assert.IsTrue(favorites.Any(a => a.ArtworkID == artworkId2), "Missing second favorite");
        }

        [Test]
        public void GetUserFavoriteArtworks_UserWithoutFavorites_ReturnsEmptyList()
        {
            // Arrange - Fresh user with no favorites
            int userId = CreateTestUser();

            // Act
            var favorites = galleryService.GetUserFavoriteArtworks(userId);

            // Assert
            Assert.IsEmpty(favorites, "Should return empty list for user with no favorites");
        }
        #endregion
    }
}
