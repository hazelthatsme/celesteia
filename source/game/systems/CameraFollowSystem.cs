using Celesteia.Game.Components;
using Celesteia.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Celesteia.Game.Systems {
    public class CameraFollowSystem : EntityUpdateSystem
    {
        private readonly Camera2D _camera;

        private ComponentMapper<Transform2> transformMapper;

        public CameraFollowSystem(Camera2D camera) : base(Aspect.All(typeof(TargetPosition), typeof(CameraFollow))) {
            _camera = camera;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            transformMapper = mapperService.GetMapper<Transform2>();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (int entityId in ActiveEntities) {
                _camera.MoveTo(transformMapper.Get(entityId).Position);
                break;
            }
        }
    }
}