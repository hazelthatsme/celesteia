using Celesteia.Game.Components;
using Celesteia.Game.World;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Celesteia.Game.Systems {
    public class TargetPositionSystem : EntityUpdateSystem {
        private GameWorld _gameWorld;
        
        private ComponentMapper<Transform2> transformMapper;
        private ComponentMapper<TargetPosition> targetPositionMapper;

        public TargetPositionSystem(GameWorld gameWorld) : base(Aspect.All(typeof(Transform2), typeof(TargetPosition))) {
            _gameWorld = gameWorld;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            transformMapper = mapperService.GetMapper<Transform2>();
            targetPositionMapper = mapperService.GetMapper<TargetPosition>();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (int entityId in ActiveEntities) {
                TargetPosition targetPosition = targetPositionMapper.Get(entityId);
                Transform2 transform = transformMapper.Get(entityId);

                if (targetPosition.Target.X < 0 || targetPosition.Target.X > _gameWorld.ChunkMap.BlockWidth)
                    targetPosition.Target.X = MathHelper.Clamp(targetPosition.Target.X, 0f, _gameWorld.ChunkMap.BlockWidth);
                
                transform.Position = targetPosition.Target;
            }
        }
    }
}