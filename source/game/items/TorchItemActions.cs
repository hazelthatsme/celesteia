using Celesteia.Game.Planets;
using Celesteia.Resources;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;

namespace Celesteia.Game.Items {
    public class TorchItemActions : BlockItemActions {
        public TorchItemActions(NamespacedKey blockKey) : base(blockKey) {}

        public override bool Secondary(GameTime gameTime, ChunkMap chunkMap, Point cursor, Entity user) => false;

        public override bool Assert(GameTime g, ChunkMap cm, Point c, Entity u, bool wa = false)
        => !cm.GetBackground(c).Empty && base.Assert(g, cm, c, u, false);
    }
}