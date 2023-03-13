using System.Diagnostics;
using Celesteia.Game;
using Celesteia.Game.Components.Items;
using MonoGame.Extended.TextureAtlases;

namespace Celesteia.Resources.Types {
    public class ItemType : IResourceType {
        public readonly byte ItemID;
        public readonly string Name;
        public readonly string Lore;
        public readonly TextureRegion2D Sprite;
        public readonly int MaxStackSize;
        public ItemActions Actions;
        public readonly bool ConsumeOnUse;

        public ItemType(byte id, string name, string lore, TextureRegion2D sprite, ItemActions actions, bool consumeOnUse, int maxStack) {
            Debug.WriteLine($"  Loading item '{name}' ({id})...");

            ItemID = id;
            Name = name;
            Lore = lore;
            Sprite = sprite;
            Actions = actions;
            MaxStackSize = maxStack;
            ConsumeOnUse = consumeOnUse;
        }

        public ItemStack GetStack(int amount) {
            return new ItemStack(ItemID, amount);
        }

        public byte GetID() => ItemID;
    }
}