using System;
using System.Diagnostics;
using Celesteia.Game.Components;
using Celesteia.Game.Components.Physics;
using Celesteia.Game.Worlds;
using Celesteia.Resources;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Celesteia.Game.Systems.Physics {
    public class PhysicsWorldCollisionSystem : EntityUpdateSystem {
        private GameWorld _gameWorld;

        public PhysicsWorldCollisionSystem(GameWorld gameWorld) : base(Aspect.All(typeof(TargetPosition), typeof(PhysicsEntity), typeof(CollisionBox))) {
            _gameWorld = gameWorld;
        }

        private ComponentMapper<Transform2> transformMapper;
        private ComponentMapper<TargetPosition> targetPositionMapper;
        private ComponentMapper<PhysicsEntity> physicsEntityMapper;
        private ComponentMapper<CollisionBox> collisionBoxMapper;

        public override void Initialize(IComponentMapperService mapperService)
        {
            transformMapper = mapperService.GetMapper<Transform2>();
            targetPositionMapper = mapperService.GetMapper<TargetPosition>();
            physicsEntityMapper = mapperService.GetMapper<PhysicsEntity>();
            collisionBoxMapper = mapperService.GetMapper<CollisionBox>();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (int entityId in ActiveEntities) {
                Transform2 transform = transformMapper.Get(entityId);
                TargetPosition targetPosition = targetPositionMapper.Get(entityId);
                PhysicsEntity physicsEntity = physicsEntityMapper.Get(entityId);
                CollisionBox collisionBox = collisionBoxMapper.Get(entityId);

                collisionBox.Update(transform.Position);

                int minX = (int)Math.Floor(collisionBox.Bounds.Center.X - (collisionBox.Bounds.Width / 2f));
                int maxX = (int)Math.Ceiling(collisionBox.Bounds.Center.X + (collisionBox.Bounds.Width / 2f));

                int minY = (int)Math.Floor(collisionBox.Bounds.Center.Y - (collisionBox.Bounds.Height / 2f));
                int maxY = (int)Math.Ceiling(collisionBox.Bounds.Center.Y + (collisionBox.Bounds.Height / 2f));
                
                bool collLeft = false;
                bool collRight = false;
                bool collUp = false;
                bool collDown = false;

                for (int i = minX; i < maxX; i++)
                    for (int j = minY; j < maxY; j++) {
                        RectangleF? blockBox = _gameWorld.GetBlockBoundingBox(i, j);
                        if (blockBox.HasValue) {
                            RectangleF inter = RectangleF.Intersection(collisionBox.Bounds, blockBox.Value);

                            if (inter.IsEmpty) continue;

                            if (inter.Width < inter.Height) {
                                collLeft = blockBox.Value.Center.X < collisionBox.Bounds.Center.X;
                                collRight = blockBox.Value.Center.X > collisionBox.Bounds.Center.X;

                                targetPosition.Target += new Vector2(blockBox.Value.Center.X < collisionBox.Bounds.Center.X ? inter.Width : -inter.Width, 0f);
                            } else {
                                collUp = blockBox.Value.Center.Y < collisionBox.Bounds.Center.Y;
                                collDown = blockBox.Value.Center.Y > collisionBox.Bounds.Center.Y;

                                targetPosition.Target += new Vector2(0f, blockBox.Value.Center.Y < collisionBox.Bounds.Center.Y ? inter.Height : -inter.Height);
                            }

                            collisionBox.Update(targetPosition.Target);
                        }
                    }
                
                physicsEntity.CollidingDown = collDown;
                physicsEntity.CollidingUp = collUp;
                physicsEntity.CollidingLeft = collLeft;
                physicsEntity.CollidingRight = collRight;

                Debug.WriteLine(collDown);
            }
        }
    }
}