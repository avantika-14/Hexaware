using System;

namespace Entity
{
    public class Artworks
    {
        private int aArtworkID;
        private string aTitle;
        private string aDescription;
        private DateTime aCreationDate;
        private string aMedium;
        private string aImageURL;
        private int aArtistID;

        // default constructor
        public Artworks() { }

        // parameterised constructors

        // Constructor without ID (used when adding a new artwork)
        public Artworks(string title, string description, DateTime creationDate, string medium, string imageURL, int artistID)
        {
            this.aTitle = title;
            this.aDescription = description;
            this.aCreationDate = creationDate;
            this.aMedium = medium;
            this.aImageURL = imageURL;
            this.aArtistID = artistID;
        }

        // Constructor with ID (used when fetching from DB)
        public Artworks(int artworkID, string title, string description, DateTime creationDate, string medium, string imageURL, int artistID)
        {
            this.aArtworkID = artworkID;
            this.aTitle = title;
            this.aDescription = description;
            this.aCreationDate = creationDate;
            this.aMedium = medium;
            this.aImageURL = imageURL;
            this.aArtistID = artistID;
        }

        // Properties
        public int ArtworkID { get { return aArtworkID; } set { aArtworkID = value; } }

        public string Title { get { return aTitle; } set { aTitle = value; }}

        public string Description { get { return aDescription; } set { aDescription = value; }}
        
        public DateTime CreationDate { get { return aCreationDate; } set {aCreationDate = value; }}
        
        public string Medium { get { return aMedium; } set { aMedium = value; } }

        public string ImageURL { get { return aImageURL; } set {aImageURL = value; }}

        public int ArtistID { get {return aArtistID; } set {aArtistID = value; }}

    }
}
