using System;
using Celesteia.Game.Components.Physics;
using Celesteia.Game.Worlds;
using Celesteia.Graphics;
using Celesteia.Resources;
using Celesteia.Resources.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace Celesteia.Game.Systems.Physics {    
    public class PhysicsCollisionDebugSystem : EntityDrawSystem
    {
        private readonly Camera2D _camera;
        private readonly SpriteBatch _spriteBatch;
        private readonly GameWorld _gameWorld;

        private ComponentMapper<Transform2> transformMapper;
        private ComponentMapper<CollisionBox> collisionBoxMapper;

        public PhysicsCollisionDebugSystem(Camera2D camera, SpriteBatch spriteBatch, GameWorld gameWorld) : base(Aspect.All(typeof(Transform2), typeof(CollisionBox))) {
            _camera = camera;
            _spriteBatch = spriteBatch;
            _gameWorld = gameWorld;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            transformMapper = mapperService.GetMapper<Transform2>();
            collisionBoxMapper = mapperService.GetMapper<CollisionBox>();
        }

        public override void Draw(GameTime gameTime)
        {
            if (!GameInstance.DebugMode) return;

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.PointClamp, null, null, null, _camera.GetViewMatrix());
            
            foreach (int entityId in ActiveEntities) {
                RectangleF box = collisionBoxMapper.Get(entityId).Bounds;
                collisionBoxMapper.Get(entityId).Blocks.ForEach(block => {
                    _spriteBatch.DrawRectangle(box, Color.Green, .05f, 0f);
                    _spriteBatch.DrawRectangle(block, )
                });

                

                int minX = (int)Math.Floor(box.Center.X - (box.Width / 2f));
                int maxX = (int)Math.Ceiling(box.Center.X + (box.Width / 2f));

                int minY = (int)Math.Floor(box.Center.Y - (box.Height / 2f));
                int maxY = (int)Math.Ceiling(box.Center.Y + (box.Height / 2f));

                for (int i = minX; i < maxX; i++)
                    for (int j = minY; j < maxY; j++) {
                        RectangleF? blockBox = _gameWorld.GetBlockBoundingBox(i, j);
                        if (blockBox.HasValue) {
                            _spriteBatch.DrawRectangle(new RectangleF(i, j, blockBox.Value.Width, blockBox.Value.Height), Color.Red, .05f, 0f);
                            _spriteBatch.DrawRectangle(collisionBoxMapper.Get(entityId).Intersection(blockBox.Value), Color.Blue, .05f, 0f);
                        }
                    }
            }

            _spriteBatch.End();
        }
    }
}