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
        private Action<Entity> InstantiateAction;

        public TileEntityType(Action<Entity> instantiate, int width, int height, int originX = 0, int originY = 0) {
            InstantiateAction = instantiate;
            Bounds = new Point(width, height);
            Origin = new Point(originX, originY);
        }

        public void Instantiate(Entity entity) {
            InstantiateAction.Invoke(entity);
        }
    }
}