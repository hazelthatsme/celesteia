using Celesteia.Game.World;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;

namespace Celesteia.Game.Items {
    public interface IItemActions {

        public bool Primary(GameTime gameTime, GameWorld world, Point cursor, Entity user);
        public bool Secondary(GameTime gameTime, GameWorld world, Point cursor, Entity user);
    }
}