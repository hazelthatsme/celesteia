using Celesteia.Game.Components.Items;

namespace Celesteia.Game.Components {
    public class EntityInventory {
        private Inventory _inventory;
        public Inventory Inventory => _inventory;

        public EntityInventory(int capacity, params ItemStack[] startingItems) {
            _inventory = new Inventory(capacity);

            for (int i = 0; i < startingItems.Length; i++)
                _inventory.AddItem(startingItems[i]);
        }
    }
}