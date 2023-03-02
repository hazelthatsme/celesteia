using Celesteia.Game.Worlds;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;

namespace Celesteia.Game {
    public class ItemActions {
        public virtual bool OnLeftClick(GameWorld world, Vector2 cursor, Entity user) { return true; }
        public virtual bool OnRightClick(GameWorld world, Vector2 cursor, Entity user) { return true; }
        public virtual bool OnLeftClickDown(GameWorld world, Vector2 cursor, Entity user) { return true; }
        public virtual bool OnRightClickDown(GameWorld world, Vector2 cursor, Entity user) { return true; }
        public virtual bool OnLeftClickUp(GameWorld world, Vector2 cursor, Entity user) { return true; }
        public virtual bool OnRightClickUp(GameWorld world, Vector2 cursor, Entity user) { return true; }
    }
}