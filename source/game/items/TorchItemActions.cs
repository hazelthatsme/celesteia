using Celesteia.Game.World;
using Celesteia.Resources;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;

namespace Celesteia.Game.Items {
    public class TorchItemActions : BlockItemActions {
        public TorchItemActions(NamespacedKey blockKey) : base(blockKey) {}

        public override bool Secondary(GameTime gameTime, GameWorld world, Point cursor, Entity user) => false;

        public override bool Assert(GameTime g, GameWorld w, Point c, Entity u, bool wa = false)
        => w.ChunkMap.GetBackground(c) != 0 && base.Assert(g, w, c, u, false);
    }
}