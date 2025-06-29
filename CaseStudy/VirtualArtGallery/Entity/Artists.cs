using System;

namespace Entity
{
    public class Artists
    {
        private int aArtistId;
        private string aName;
        private string aBiography;
        private DateTime aBirthDate;
        private string aNationality;
        private string aWebsite;
        private string aContactInformation;

        // default constructor
        public Artists() { }

        // parameterised constructor
        public Artists(int artistId, string name, string biography, DateTime birthDate, string nationality, string website, string contactInformation)
        {
            this.aArtistId = artistId;
            this.aName = name;
            this.aBiography = biography;
            this.aBirthDate = birthDate;
            this.aNationality = nationality;
            this.aWebsite = website;
            this.aContactInformation = contactInformation;
        }

        public int ArtistID { get { return aArtistId; } set { aArtistId = value; } }

        public string Name { get { return aName; } set { aName = value; } } 
        
        public string Biography { get { return aBiography; } set { aBiography = value; } }

        public DateTime BirthDate { get { return aBirthDate; } set { aBirthDate = value; } } 

        public string Nationality { get { return aNationality; } set { aNationality = value; } }

        public string Website { get { return aWebsite; } set { aWebsite = value; } }

        public string ContactInformation { get { return aContactInformation; } set { aContactInformation = value; } }
    }
}
