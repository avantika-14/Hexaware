using EntityEF;

namespace DAOEF
{
    public interface IVirtualArtGalleryEF
    {
        // gallery management
        bool AddGallery(Gallery Gallery);

        bool UpdateGallery(Gallery Gallery);

        Gallery GetGalleryByID(int GalleryID);
    }
}
