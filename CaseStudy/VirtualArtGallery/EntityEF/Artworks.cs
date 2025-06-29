using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EntityEF
{
    [Table("artworks")]
    public class Artworks
    {
        [Key]
        public int ArtworkID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreationDate { get; set; }

        public string Medium { get; set; }

        public string ImageURL { get; set; }

        [ForeignKey("Artist")]
        [Column("artistID")]
        public int ArtistID { get; set; }
        public virtual Artists Artist { get; set; }

        public virtual ICollection<Gallery> Gallery { get; set; }

        public virtual ICollection<Users> FavoriteByUsers { get; set; }
    }
}
