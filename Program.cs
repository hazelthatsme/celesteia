using System;

namespace Celesteia
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new GameInstance())
                game.Run();
        }
    }
}
