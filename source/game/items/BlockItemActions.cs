using Celesteia.Game.Components;
using Celesteia.Game.Components.Physics;
using Celesteia.Game.Worlds;
using Celesteia.Resources;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;

namespace Celesteia.Game.Items {
    public class BlockItemActions : CooldownItemActions {
        private NamespacedKey _blockKey;
        private byte _block = 0;
        private NamespacedKey BlockKey { get => _blockKey; set {
            _blockKey = value;
            _block = ResourceManager.Blocks.GetResource(_blockKey).GetID();
        }}

        public BlockItemActions(NamespacedKey blockKey) {
            UseTime = 0.2;
            BlockKey = blockKey;
        }
        
        public override bool Primary(GameTime gameTime, GameWorld world, Vector2 cursor, Entity user)
        => Assert(gameTime, world, cursor, user, false) && Place(world, cursor, false);

        public override bool Secondary(GameTime gameTime, GameWorld world, Vector2 cursor, Entity user)
        => Assert(gameTime, world, cursor, user, true) && Place(world, cursor, true);

        public virtual bool Assert(GameTime gameTime, GameWorld world, Vector2 cursor, Entity user, bool forWall) {
            if (!base.Assert(gameTime)) return false;
            if (_block == 0) return false;

            UpdateLastUse(gameTime);

            if (!user.Has<Transform2>() || !user.Has<EntityAttributes>()) return false;

            Transform2 entityTransform = user.Get<Transform2>();
            EntityAttributes.EntityAttributeMap attributes = user.Get<EntityAttributes>().Attributes;

            if (Vector2.Distance(entityTransform.Position, cursor) > attributes.Get(EntityAttribute.BlockRange)) return false;

            if (!forWall && user.Has<CollisionBox>()) {
                Rectangle box = user.Get<CollisionBox>().Rounded;
                RectangleF? rect = world.TestBoundingBox(cursor.ToPoint(), _block);
                if (rect.HasValue) {
                    bool intersect = rect.Intersects(new RectangleF(box.X, box.Y, box.Width, box.Height));
                    if (intersect) return false;
                }
            }

            // If the current tile of the chosen layer is already occupied, don't place the block.
            if ((forWall && world.GetWallBlock(cursor) != 0) || (!forWall && world.GetBlock(cursor) != 0)) return false;

            if (!world.GetAnyBlock(cursor + new Vector2(-1, 0), true) && 
                !world.GetAnyBlock(cursor + new Vector2(1, 0), true) &&
                !world.GetAnyBlock(cursor + new Vector2(0, -1), true) && 
                !world.GetAnyBlock(cursor + new Vector2(0, 1), true)) {
                if (!forWall && world.GetWallBlock(cursor) == 0) return false;
                else if (forWall && world.GetBlock(cursor) == 0) return false;
            }
            
            return true;
        }

        public bool Place(GameWorld world, Vector2 cursor, bool wall) {
            if (wall) world.SetWallBlock(cursor, _block);
            else world.SetBlock(cursor, _block);

            world.GetChunk(ChunkVector.FromVector2(cursor)).DoUpdate = true;

            return true;
        }
    }
}