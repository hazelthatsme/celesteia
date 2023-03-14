using Celesteia.Resources;
using Celesteia.Resources.Types;

namespace Celesteia.Game.Components.Items {
    public class ItemStack {
        public byte ID;
        public NamespacedKey Key;
        public int Amount;
        public readonly ItemType Type;

        public ItemStack(NamespacedKey key, int amount) {
            Key = key;
            Amount = amount;

            Type = ResourceManager.Items.GetResource(key) as ItemType;
        }

        public ItemStack NewStack() {
            ItemStack stack = null;
            if (Amount > Type.MaxStackSize) stack = new ItemStack(Key, Amount - Type.MaxStackSize);

            return stack;
        }

        public ItemStack Clone() {
            return new ItemStack(Key, Amount);
        }

        public override string ToString()
        {
            return $"{Amount}x {Type.Name}";
        }
    }
}