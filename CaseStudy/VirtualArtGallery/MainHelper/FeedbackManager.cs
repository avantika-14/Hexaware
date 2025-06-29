using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualArtGallery.MainHelper
{
    public static class FeedbackManager
    {
        public static void ShowSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
            SoundManager.PlaySuccess();
        }

        public static void ShowWarning(string message)
        {
            Console.ForegroundColor= ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
            SoundManager.PlayWarning();
        } 
        public static void ShowError(string message)
        {
            Console.ForegroundColor= ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
            SoundManager.PlayError();
            ConsoleHelper.WaitForUser();
        }
    }
}
