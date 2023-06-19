using Celesteia.Game.ECS;

namespace Celesteia.Game.Components.Entity {
    public class GameWorldEntity {
        private GameWorld _w;
        private int _id;
        public GameWorldEntity(GameWorld w, int id) {
            _w = w;
            _id = id;
        }

        public void Destroy() => _w.DestroyEntity(_id);
    }
}