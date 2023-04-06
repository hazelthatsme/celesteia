using System;
using MonoGame.Extended.Entities;

namespace Celesteia.Resources.Types {
    public class EntityType : IResourceType {
        private byte id;
        public byte GetID() => id;
        public void SetID(byte value) => id = value;

        private Action<World, Entity> InstantiateAction;

        public EntityType(Action<World, Entity> instantiate) {
            InstantiateAction = instantiate;
        }

        public void Instantiate(World world, Entity entity) {
            InstantiateAction.Invoke(world, entity);
        }
    }
}