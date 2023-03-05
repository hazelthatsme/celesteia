using Celesteia.Game.Worlds;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;

namespace Celesteia.Game {
    public class ItemActions {
        public double UseTime = 1.0;
        public double LastUse = 0.0;
        public void UpdateLastUse(GameTime gameTime) {
            LastUse = gameTime.TotalGameTime.TotalSeconds;
        }
        public bool CheckUseTime(GameTime gameTime) {
            return gameTime.TotalGameTime.TotalSeconds - LastUse >= UseTime;
        }

        public virtual bool OnLeftClick(GameTime gameTime, GameWorld world, Vector2 cursor, Entity user) { return true; }
        public virtual bool OnRightClick(GameTime gameTime, GameWorld world, Vector2 cursor, Entity user) { return true; }
        public virtual bool OnLeftClickDown(GameTime gameTime, GameWorld world, Vector2 cursor, Entity user) { return true; }
        public virtual bool OnRightClickDown(GameTime gameTime, GameWorld world, Vector2 cursor, Entity user) { return true; }
        public virtual bool OnLeftClickUp(GameTime gameTime, GameWorld world, Vector2 cursor, Entity user) { return true; }
        public virtual bool OnRightClickUp(GameTime gameTime, GameWorld world, Vector2 cursor, Entity user) { return true; }
    }
}