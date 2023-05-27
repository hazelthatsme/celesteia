using Celesteia.Game.Worlds;
using Celesteia.Resources;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;

namespace Celesteia.Game.Items {
    public class TorchItemActions : BlockItemActions {
        public TorchItemActions(NamespacedKey blockKey) : base(blockKey) {}

        public override bool Assert(GameTime gameTime, GameWorld world, Vector2 cursor, Entity user, bool forWall) {
            if (world.GetWallBlock(cursor) == 0) return false;

            return base.Assert(gameTime, world, cursor, user, false);
        }
    }
}