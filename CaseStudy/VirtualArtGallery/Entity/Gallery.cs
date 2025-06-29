namespace Entity
{
    public class Gallery
    {
        private int gGalleryId;
        private string gName;
        private string gDescription;
        private string gLocation;
        private int gCurator;
        private string gOpeningHours;

        // default
        public Gallery() { }

        // parameterised
        public Gallery(int galleryId, string name, string description, string location, int curator, string openingHours)
        {
            this.gGalleryId = galleryId;
            this.gName = name;
            this.gDescription = description;
            this.gLocation = location;
            this.gCurator = curator;
            this.gOpeningHours = openingHours;
        }

        public int GalleryID { get { return gGalleryId; } set { gGalleryId = value; } }

        public string Name { get { return gName; } set { gName = value; } }

        public string Description { get { return gDescription; } set { gDescription = value; } }

        public string Location { get { return gLocation; } set { gLocation = value; } }

        public int Curator { get { return gCurator; } set { gCurator = value; } }

        public string OpeningHours { get { return gOpeningHours; } set {gOpeningHours = value; } }  
    }
}
