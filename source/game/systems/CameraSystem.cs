using System;
using Celesteia.Game.Components;
using Celesteia.Game.Input;
using Celesteia.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Celesteia.Game.Systems {
    public class CameraSystem : EntityUpdateSystem
    {
        private Camera2D _camera;
        private float smoothing = 128f;

        private ComponentMapper<Transform2> transformMapper;

        public CameraSystem(Camera2D camera) : base(Aspect.All(typeof(Transform2), typeof(CameraFollow)))
        => _camera = camera;

        public override void Initialize(IComponentMapperService mapperService)
        => transformMapper = mapperService.GetMapper<Transform2>();

        Vector2 pos;
        public override void Update(GameTime gameTime)
        {
            foreach (int entityId in ActiveEntities) {
                pos = transformMapper.Get(entityId).Position * smoothing;
                pos.X = MathF.Round(pos.X) / smoothing;
                pos.Y = MathF.Round(pos.Y) / smoothing;
                _camera.Center = pos;
                break;
            }
            
            if (KeyboardHelper.IsDown(Keys.LeftControl)) _camera.Zoom += (int) Math.Clamp(MouseHelper.ScrollDelta, -1f, 1f);
        }
    }
}