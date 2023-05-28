using Celesteia.Game.Components;
using Celesteia.Game.Components.Physics;
using Celesteia.Game.World;
using Celesteia.Resources;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;

namespace Celesteia.Game.Items {
    public class BlockItemActions : CooldownItemActions {
        private byte _block = 0;

        public BlockItemActions(NamespacedKey blockKey) {
            UseTime = 0.2;
            _block = ResourceManager.Blocks.GetResource(blockKey).GetID();
        }
        
        public override bool Primary(GameTime gameTime, GameWorld world, Point cursor, Entity user)
        => Assert(gameTime, world, cursor, user, false) && Place(world, cursor, false);

        public override bool Secondary(GameTime gameTime, GameWorld world, Point cursor, Entity user)
        => Assert(gameTime, world, cursor, user, true) && Place(world, cursor, true);

        public virtual bool Assert(GameTime gameTime, GameWorld world, Point cursor, Entity user, bool forWall) {
            if (!base.Assert(gameTime)) return false;
            if (_block == 0) return false;

            UpdateLastUse(gameTime);

            if (!user.Has<Transform2>() || !user.Has<EntityAttributes>()) return false;

            Transform2 entityTransform = user.Get<Transform2>();
            EntityAttributes.EntityAttributeMap attributes = user.Get<EntityAttributes>().Attributes;

            if (Vector2.Distance(entityTransform.Position, cursor.ToVector2()) > attributes.Get(EntityAttribute.BlockRange)) return false;

            if (!forWall && user.Has<CollisionBox>()) {
                Rectangle box = user.Get<CollisionBox>().Rounded;
                RectangleF? rect = world.TestBoundingBox(cursor.X, cursor.Y, _block);
                if (rect.HasValue) {
                    bool intersect = rect.Intersects(new RectangleF(box.X, box.Y, box.Width, box.Height));
                    if (intersect) return false;
                }
            }

            // If the current tile of the chosen layer is already occupied, don't place the block.
            if ((forWall ? world.ChunkMap.GetBackground(cursor) : world.ChunkMap.GetForeground(cursor)) != 0) return false;
            
            return (
                world.ChunkMap.GetAny(cursor.X - 1, cursor.Y) ||
                world.ChunkMap.GetAny(cursor.X + 1, cursor.Y) ||
                world.ChunkMap.GetAny(cursor.X, cursor.Y - 1) || 
                world.ChunkMap.GetAny(cursor.X, cursor.Y + 1)
            ) || (forWall ? world.ChunkMap.GetForeground(cursor) : world.ChunkMap.GetBackground(cursor)) != 0;
        }

        public bool Place(GameWorld world, Point cursor, bool wall) {
            if (wall) world.ChunkMap.SetBackground(cursor, _block);
            else world.ChunkMap.SetForeground(cursor, _block);

            return true;
        }
    }
}