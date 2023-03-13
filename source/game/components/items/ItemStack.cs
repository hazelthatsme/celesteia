using Celesteia.Resources;
using Celesteia.Resources.Management;

namespace Celesteia.Game.Components.Items {
    public class ItemStack {
        public byte ID;
        public int Amount;
        public readonly ItemType Type;

        public ItemStack(byte id, int amount) {
            ID = id;
            Amount = amount;

            Type = ResourceManager.Items.GetItem(id);
        }

        public ItemStack NewStack() {
            ItemStack stack = null;
            if (Amount > Type.MaxStackSize) stack = new ItemStack(Type.ItemID, Amount - Type.MaxStackSize);

            return stack;
        }

        public ItemStack Clone() {
            return new ItemStack(ID, Amount);
        }

        public override string ToString()
        {
            return $"{Amount}x {Type.Name}";
        }
    }
}