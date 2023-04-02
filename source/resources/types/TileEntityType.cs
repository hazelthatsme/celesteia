using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;

namespace Celesteia.Resources.Types {
    public class TileEntityType : IResourceType {
        private byte id;
        public byte GetID() => id;
        public void SetID(byte value) => id = value;

        public readonly Point Bounds;
        public readonly Point Origin;
        public readonly NamespacedKey PartKey;
        private Action<Entity> InstantiateAction;

        public TileEntityType(NamespacedKey part, Action<Entity> instantiate, int width, int height, int originX = 0, int originY = 0) {
            PartKey = part;
            InstantiateAction = instantiate;
            Bounds = new Point(width, height);
            Origin = new Point(originX, originY);
        }

        public void Instantiate(Entity entity) {
            InstantiateAction.Invoke(entity);
        }
    }
}