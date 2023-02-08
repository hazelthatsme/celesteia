using System;
using MonoGame.Extended.Entities;

namespace Celestia.Utilities.ECS {
    public class EntityBuilder : IDisposable {
        private Entity entity;

        public EntityBuilder(World _world) {
            entity = _world.CreateEntity();
        }

        public EntityBuilder AddComponent<T>(T component) where T : class {
            entity.Attach<T>(component);
            return this;
        }

        public Entity Build() {
            return entity;
        }

        public void Dispose()
        {
            entity = null;
        }
    }
}