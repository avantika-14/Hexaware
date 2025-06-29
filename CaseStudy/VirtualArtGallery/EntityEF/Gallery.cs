using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EntityEF
{

    [Table("gallery")]
    public class Gallery
    {
        [Key]
        public int GalleryID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public string OpeningHours { get; set; }

        [ForeignKey("Curator")]
        [Column("curator")]
        public int CuratorID { get; set; }
        public virtual Artists Curator { get; set; }

        public virtual ICollection<Artworks> Artworks { get; set; }
    }
}
