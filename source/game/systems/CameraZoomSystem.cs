using Celesteia.Game.Input;
using Celesteia.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Entities.Systems;

namespace Celesteia.Game.Systems {
    public class CameraZoomSystem : UpdateSystem 
    {
        private readonly Camera2D _camera;
        
        public CameraZoomSystem(Camera2D camera) => _camera = camera;

        public override void Update(GameTime gameTime) {
            if (!KeyboardWrapper.GetKeyHeld(Keys.LeftControl)) return;
            _camera.Zoom += MouseWrapper.GetScrollDelta();
        }
    }
}