using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EntityEF
{
    [Table("artwork_gallery")]
    public class ArtworkGallery
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("ArtworkID")] 
        [Column("artworkID")]
        public virtual Artworks Artworks { get; set; }
        public int ArtworkID { get; set; }

        [ForeignKey("GalleryID")]
        [Column ("galleryID")]
        public virtual Gallery Gallery { get; set; }
        public int GalleryID { get; set; }
    }
}
