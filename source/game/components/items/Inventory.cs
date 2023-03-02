using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Celesteia.Game.Components.Items {
    public class Inventory {
        private ItemStack[] items;
        public readonly int Capacity;

        public Inventory(int slots = 27) {
            Capacity = slots;
            items = new ItemStack[slots];
        }

        // Try adding an item to the inventory, return false if the inventory has no capacity for this action.
        public bool AddItem(ItemStack stack) {
            ItemStack existingStack = GetItemStackWithID(stack.ID);

            // If an item stack with this ID already exists, add the amount to that stack.
            if (existingStack != null) {
                existingStack.Amount += stack.Amount;

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
            return Array.FindLast(items, x => x != null && x.ID == id && x.Amount < x.Type.MaxStackSize);
        }

        public ItemStack GetSlot(int slot) {
            if (slot < 0 || slot > Capacity - 1) throw new ArgumentException($"Slot {slot} falls outside of the inventory's capacity.");
            return items[slot];
        }

        private void AddItemStack(ItemStack newStack) {
            if (!HasCapacity()) return;
            int i = Array.FindIndex(items, x => x == null);
            items[i] = newStack;
        }

        private bool HasCapacity() {
            return Array.Exists(items, x => x == null);
        }
    }
}