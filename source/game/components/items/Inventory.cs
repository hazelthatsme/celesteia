using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Celesteia.Game.Components.Items {
    public class Inventory {
        private List<ItemStack> items;
        public readonly int Capacity;

        public Inventory(int slots = 27) {
            Capacity = slots;
            items = new List<ItemStack>();
        }

        // Try adding an item to the inventory, return false if the inventory has no capacity for this action.
        public bool AddItem(ItemStack stack) {
            ItemStack existingStack = GetItemStackWithID(stack.ID);

            // If an item stack with this ID already exists, add the amount to that stack.
            if (existingStack != null) {
                existingStack.Amount += stack.Amount;

                Debug.WriteLine($"Obtained {stack.Amount}x {stack.Type.Name}.");

                // If the max stack size was exceeded and a new stack has to be created, create it.
                ItemStack newStack = existingStack.NewStack();
                if (newStack != null) {
                    if (!HasCapacity()) return false;
                    existingStack.Amount = existingStack.Type.MaxStackSize;
                    AddItemStack(newStack);
                }
            } else AddItemStack(stack);

            return true;
        }

        public ItemStack GetItemStackWithID(byte id) {
            return items.FindLast(x => x.ID == id && x.Amount < x.Type.MaxStackSize);
        }

        private void AddItemStack(ItemStack newStack) {
            items.Add(newStack);
        }

        private bool HasCapacity() {
            return items.Count < Capacity;
        }

        public void DebugOutput()
        {
            Debug.WriteLine("Stacks in inventory:");
            for (int i = 0; i < items.Count; i++)
                Debug.WriteLine($" - {items[i].Type.Name} x{items[i].Amount}");
        }
    }
}