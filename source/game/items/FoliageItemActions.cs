using Celesteia.Game.World;
using Celesteia.Resources;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;

namespace Celesteia.Game.Items {
    public class FoliageItemActions : BlockItemActions {
        byte grown_soil;
        public FoliageItemActions(NamespacedKey blockKey) : base(blockKey) {
            grown_soil = ResourceManager.Blocks.GetResource(NamespacedKey.Base("grown_soil")).GetID();
        }

        public override bool Secondary(GameTime gameTime, GameWorld world, Point cursor, Entity user) => false;

        public override bool Assert(GameTime gameTime, GameWorld world, Point cursor, Entity user, bool forWall)
        => world.ChunkMap.GetForeground(cursor.X, cursor.Y + 1) == grown_soil && base.Assert(gameTime, world, cursor, user, false);
    }
}