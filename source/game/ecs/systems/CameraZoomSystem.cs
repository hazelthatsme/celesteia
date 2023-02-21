using Celesteia.Game.Input;
using Celesteia.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities.Systems;

namespace Celesteia.Game.ECS.Systems {
    public class CameraZoomSystem : UpdateSystem 
    {
        private readonly Camera2D _camera;
        public CameraZoomSystem(Camera2D camera) => _camera = camera;
        public override void Update(GameTime gameTime) => _camera.Zoom += MouseWrapper.GetScrollDelta();
    }
}