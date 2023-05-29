using Celesteia.Game.Components;
using Celesteia.Game.Components.Physics;
using Celesteia.Game.Planets;
using Celesteia.Resources;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;

namespace Celesteia.Game.Items {
    public class BlockItemActions : CooldownItemActions {
        protected byte _block = 0;

        public BlockItemActions(NamespacedKey blockKey) {
            UseTime = 0.2;
            _block = ResourceManager.Blocks.GetResource(blockKey).GetID();
        }
        
        public override bool Primary(GameTime gameTime, ChunkMap chunkMap, Point cursor, Entity user)
        => Assert(gameTime, chunkMap, cursor, user, false) && Place(chunkMap, cursor, false);

        public override bool Secondary(GameTime gameTime, ChunkMap chunkMap, Point cursor, Entity user)
        => Assert(gameTime, chunkMap, cursor, user, true) && Place(chunkMap, cursor, true);

        public virtual bool Assert(GameTime gameTime, ChunkMap chunkMap, Point cursor, Entity user, bool forWall) {
            if (!base.Assert(gameTime)) return false;
            if (_block == 0) return false;

            UpdateLastUse(gameTime);

            if (!user.Has<Transform2>() || !user.Has<EntityAttributes>()) return false;

            Transform2 entityTransform = user.Get<Transform2>();
            EntityAttributes.EntityAttributeMap attributes = user.Get<EntityAttributes>().Attributes;

            if (Vector2.Distance(entityTransform.Position, cursor.ToVector2()) > attributes.Get(EntityAttribute.BlockRange)) return false;

            if (!forWall && user.Has<CollisionBox>()) {
                Rectangle box = user.Get<CollisionBox>().Rounded;
                RectangleF? rect = chunkMap.TestBoundingBox(cursor.X, cursor.Y, _block);
                if (rect.HasValue) {
                    bool intersect = rect.Intersects(new RectangleF(box.X, box.Y, box.Width, box.Height));
                    if (intersect) return false;
                }
            }

            // If the current tile of the chosen layer is already occupied, don't place the block.
            if ((forWall ? chunkMap.GetBackground(cursor) : chunkMap.GetForeground(cursor)) != 0) return false;
            
            return (
                chunkMap.GetAny(cursor.X - 1, cursor.Y) ||
                chunkMap.GetAny(cursor.X + 1, cursor.Y) ||
                chunkMap.GetAny(cursor.X, cursor.Y - 1) || 
                chunkMap.GetAny(cursor.X, cursor.Y + 1)
            ) || (forWall ? chunkMap.GetForeground(cursor) : chunkMap.GetBackground(cursor)) != 0;
        }

        public bool Place(ChunkMap chunkMap, Point cursor, bool wall) {
            if (wall) chunkMap.SetBackground(cursor, _block);
            else chunkMap.SetForeground(cursor, _block);

            return true;
        }
    }
}