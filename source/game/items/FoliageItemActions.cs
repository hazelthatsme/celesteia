using Celesteia.Game.Worlds;
using Celesteia.Resources;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;

namespace Celesteia.Game {
    public class FoliageItemActions : BlockItemActions {
        public FoliageItemActions(NamespacedKey blockKey) : base(blockKey) {}
        
        public override bool OnRightClick(GameTime gameTime, GameWorld world, Vector2 cursor, Entity user) => false;

        public override bool Assert(GameTime gameTime, GameWorld world, Vector2 cursor, Entity user, bool forWall) {
            if (world.GetBlock(new Vector2(cursor.X, cursor.Y + 1)) != ResourceManager.Blocks.GetResource(NamespacedKey.Base("grown_soil")).GetID()) return false;

            return base.Assert(gameTime, world, cursor, user, false);
        }
    }
}