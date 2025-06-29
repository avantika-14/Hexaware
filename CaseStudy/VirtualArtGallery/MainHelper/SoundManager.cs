using System.Media;

namespace VirtualArtGallery.MainHelper
{

    public static class SoundManager
    {
        public static void PlaySuccess() => SystemSounds.Exclamation.Play();
        
        public static void PlayWarning() => SystemSounds.Question.Play();
        
        public static void PlayError() => SystemSounds.Hand.Play();

        public static void PlayMenuSelection () => SystemSounds.Beep.Play();
    }
}
