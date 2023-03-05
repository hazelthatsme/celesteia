using System;
using MonoGame.Extended.Entities;

namespace Celesteia.Game.ECS {
    /*
        Class for easily building MonoGameExtended entities.
    */

    public class EntityBuilder : IDisposable {
        private Entity _me;

        public EntityBuilder(World _world) {
            _me = _world.CreateEntity();
        }

        // Add a component to the entity.
        public EntityBuilder AddComponent<T>(T component) where T : class {
            _me.Attach<T>(component);
            return this;
        }

        // Return the built entity.
        public Entity Build() {
            return _me;
        }

        // Dispose the built entity.
        public void Dispose()
        {
            _me = null;
        }
    }
}