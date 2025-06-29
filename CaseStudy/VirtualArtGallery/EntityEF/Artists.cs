using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EntityEF
{
    [Table("artists")]
    public class Artists
    {
        [Key]
        public int ArtistID { get; set; }

        public string Name { get; set; }

        public string Biography { get; set; }

        public DateTime BirthDate { get; set; }

        public string Nationality { get; set; }

        public string Website { get; set; }

        public string ContactInformation { get; set; }

        public virtual ICollection<Artworks> Artworks { get; set; }

        public virtual ICollection<Gallery> CuratedGallery { get; set; }
    }
}
