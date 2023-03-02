using Celesteia.Game.Components.Items;

namespace Celesteia.Game.Components {
    public class EntityInventory {
        private Inventory _inventory;
        public Inventory Inventory => _inventory;

        public EntityInventory(params ItemStack[] startingItems) {
            _inventory = new Inventory();

            for (int i = 0; i < startingItems.Length; i++)
                _inventory.AddItem(startingItems[i]);
        }
    }
}