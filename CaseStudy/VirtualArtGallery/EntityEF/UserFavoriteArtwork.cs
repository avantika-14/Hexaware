using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace EntityEF
{
    [Table("user_favorite_artworks")]
    public class UserFavoriteArtwork
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("UserID")]
        [Column ("userID")]
        public virtual Users Users { get; set; }
        public int UserID { get; set; }

        [ForeignKey("ArtworkID")]
        [Column("artworkID")]
        public virtual Artworks Artworks { get; set; }

        public int ArtworkID { get; set; }

    }
}
