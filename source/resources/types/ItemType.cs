using Celesteia.Game.Items;
using MonoGame.Extended.TextureAtlases;

namespace Celesteia.Resources.Types {
    public class ItemType : IResourceType {
        private byte id;
        public readonly string Name;
        public byte GetID() => id;
        public void SetID(byte value) => id = value;

        public ItemType(string name) {
            Name = name;
        }

        public string Lore = "";
        public TextureRegion2D Sprite = null;
        public int MaxStackSize = 99;
        public IItemActions Actions;
        public bool ConsumeOnUse;
    }
}