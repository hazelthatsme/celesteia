using System.Diagnostics;
using Celesteia.Game.Input;
using Celesteia.Game.Worlds;
using Celesteia.Graphics;
using Celesteia.Resources.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;
using MonoGame.Extended.Input;

namespace Celesteia.Game.Systems {
    public class MouseClickSystem : UpdateSystem
    {
        private readonly Camera2D _camera;
        private readonly GameWorld _world;

        public MouseClickSystem(Camera2D camera, GameWorld world) {
            _camera = camera;
            _world = world;
        }

        public override void Update(GameTime gameTime)
        {
            if (MouseWrapper.GetMouseHeld(MouseButton.Left)) {
                Vector2 point = _camera.ScreenToWorld(MouseWrapper.GetPosition());
                _world.RemoveBlock(point);
            }
        }
    }
}