using System.Diagnostics;
using Microsoft.Xna.Framework.Input;

namespace Celestia.UI {
    public class PauseMenu : Menu {
        public PauseMenu() {
            buttons.Add(new Button(new ScreenSpaceRect(0.25f, 0.25f, 0.5f, 0.1f), (position) => { Debug.WriteLine("Screen space button clicked: " + position); }, "Test!"));
            Debug.WriteLine("Pause Menu initialized.");
        }
    }
}