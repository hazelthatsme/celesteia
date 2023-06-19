using Celesteia.Game.Planets;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;

namespace Celesteia.Game.Items {
    public class CooldownItemActions : IItemActions {
        public double UseTime = 1.0;
        public double LastUse = 0.0;
        public void UpdateLastUse(GameTime gameTime) => LastUse = gameTime.TotalGameTime.TotalSeconds;
        public bool CheckUseTime(GameTime gameTime) => gameTime.TotalGameTime.TotalSeconds - LastUse >= UseTime;

        public virtual bool Assert(GameTime gameTime) => CheckUseTime(gameTime);

        public virtual bool Primary(GameTime gameTime, ChunkMap chunkMap, Point cursor, Entity user) => false;
        public virtual bool Secondary(GameTime gameTime, ChunkMap chunkMap, Point cursor, Entity user) => false;
    }
}