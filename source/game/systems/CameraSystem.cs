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
        private InputManager _input;
        private float smoothing = 64f;

        private ComponentMapper<Transform2> transformMapper;

        public CameraSystem(Camera2D camera, InputManager input) : base(Aspect.All(typeof(TargetPosition), typeof(CameraFollow))) {
            _camera = camera;
            _input = input;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            transformMapper = mapperService.GetMapper<Transform2>();
        }

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