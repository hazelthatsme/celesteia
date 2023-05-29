using Celesteia.Game.Planets;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;

namespace Celesteia.Game.Items {
    public interface IItemActions {

        public bool Primary(GameTime gameTime, ChunkMap chunkMap, Point cursor, Entity user);
        public bool Secondary(GameTime gameTime, ChunkMap chunkMap, Point cursor, Entity user);
    }
}