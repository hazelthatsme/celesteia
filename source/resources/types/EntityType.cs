using System;
using MonoGame.Extended.Entities;

namespace Celesteia.Resources.Types {
    public class EntityType : IResourceType {
        private byte id;
        public byte GetID() => id;
        public void SetID(byte value) => id = value;

        private Action<Entity> InstantiateAction;

        public EntityType(Action<Entity> instantiate) {
            InstantiateAction = instantiate;
        }

        public void Instantiate(Entity entity) {
            InstantiateAction.Invoke(entity);
        }
    }
}